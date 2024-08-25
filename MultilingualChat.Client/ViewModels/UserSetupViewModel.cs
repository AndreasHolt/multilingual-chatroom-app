using System.Collections.ObjectModel;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.SignalR.Protocol;
using ReactiveUI;

namespace MultilingualChat.Client.ViewModels;

public class UserSetupViewModel : ReactiveObject
{
    public bool IsSetupCompleted { get; private set; } = false;

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

    
    public string SelectedLanguageName { get; set; }

    public bool ConfirmCommand()
    {
        )
    }
    
    // public ObservableCollection<Language> LanguageList { get; set; }
    public ObservableCollection<Language> Languages { get; set; }
    public ReactiveCommand<Unit, UserSetupResult> StartChatCommand { get; }

    public UserSetupResult StartChat()
    {
        IsSetupCompleted = true;
        return new UserSetupResult() { Username = Username, Language = SelectedLanguageName };
    }

}

public class UserSetupResult
{
    public string Username { get; set; }
    public string Language { get; set; }
}