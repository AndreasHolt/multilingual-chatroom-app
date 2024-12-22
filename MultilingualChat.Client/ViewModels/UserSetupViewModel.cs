using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MultilingualChat.Client.Services;
using ReactiveUI;

namespace MultilingualChat.Client.ViewModels;

public class UserSetupViewModel : ReactiveObject, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    private bool _isJoiningRoom;


    public bool IsJoiningRoom
    {
        get => _isJoiningRoom;
        set
        {
            _isJoiningRoom = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsJoiningRoom)));
        }
    }

    public string RoomId { get; set; }
    public string UserColor { get; set; } 

    private readonly SignalRService _signalRService;

    public UserSetupViewModel(SignalRService signalRService)
    {
        _signalRService = signalRService;

        LanguageList = new ObservableCollection<Language>(
            CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Where(c => c.Name != "")
                .OrderBy(c => c.EnglishName)
                .Select(c => new Language(c.EnglishName))
                .Distinct(new LanguageComparer())
        );

    }

    private string _username;

    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string _selectedLanguage;

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set => this.RaiseAndSetIfChanged(ref _selectedLanguage, value);
    }

    public Language SelectedLanguageName { get; set; }

    public event Action<UserSetupResult> UserConfirmed;

    public async Task ConfirmCommand()
    {
        if (_isJoiningRoom && string.IsNullOrEmpty(RoomId)) // If user is joining a room, they must enter a room id
        {
            return;
        }

        if (!string.IsNullOrEmpty(Username) || SelectedLanguageName != null)
        {
            if (!_isJoiningRoom)
            {
                RoomId = Guid.NewGuid().ToString();
            }
            
            await _signalRService.StartConnectionAsync(Username, SelectedLanguageName.LanguageName, RoomId, UserColor);

            UserConfirmed?.Invoke(new UserSetupResult()
                { Username = Username, Language = SelectedLanguageName.LanguageName, RoomId = RoomId, UserColor = UserColor});
            Console.WriteLine("Username is " + Username);
            Console.WriteLine("Selected language is " + SelectedLanguageName.LanguageName);
        }
    }

    public ObservableCollection<Language> LanguageList { get; set; }

    public UserSetupResult StartChat()
    {
        return new UserSetupResult()
            { Username = Username, Language = SelectedLanguageName.LanguageName, RoomId = RoomId, UserColor = UserColor};
    }
}

public class UserSetupResult
{
    public string Username { get; set; }
    public string Language { get; set; }
    public string RoomId { get; set; }
    public string UserColor { get; set; }
    
}

public class LanguageComparer : IEqualityComparer<Language>
{
    public bool Equals(Language x, Language y)
    {
        return x.LanguageName == y.LanguageName;
    }

    public int GetHashCode(Language obj)
    {
        return obj.LanguageName.GetHashCode();
    }
}