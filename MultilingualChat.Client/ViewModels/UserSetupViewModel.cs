using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ReactiveUI;

namespace MultilingualChat.Client.ViewModels;

public class UserSetupViewModel : ReactiveObject
{
    public UserSetupViewModel()
    {
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

    public void ConfirmCommand()
    {
        if (!string.IsNullOrEmpty(Username) || SelectedLanguageName != null)
        {
            UserConfirmed?.Invoke(new UserSetupResult()
                { Username = Username, Language = SelectedLanguageName.LanguageName });
            Console.WriteLine("Username is " + Username);
            Console.WriteLine("Selected language is " + SelectedLanguageName.LanguageName);
        }
    }

    public ObservableCollection<Language> LanguageList { get; set; }

    public UserSetupResult StartChat()
    {
        return new UserSetupResult() { Username = Username, Language = SelectedLanguageName.LanguageName };
    }
}

public class UserSetupResult
{
    public string Username { get; set; }
    public string Language { get; set; }
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