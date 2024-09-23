using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.SignalR.Protocol;
using ReactiveUI;

namespace MultilingualChat.Client.ViewModels;

public class UserSetupViewModel : ReactiveObject
{
    public UserSetupViewModel()
    {
        LanguageList = new ObservableCollection<Language>(new List<Language>
        {
            new Language("Danish"),
            new Language("English"),
            
        });
    }
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

    
    public Language SelectedLanguageName { get; set; }

    public bool ConfirmCommand()
    {
        Console.WriteLine("Username is " + Username);
        Console.WriteLine("Selected language is " + SelectedLanguageName.LanguageName);
        return true;
    }
    
    // public ObservableCollection<Language> LanguageList { get; set; }
    public ObservableCollection<Language> LanguageList { get; set; }
    public ReactiveCommand<Unit, UserSetupResult> StartChatCommand { get; }

    public UserSetupResult StartChat()
    {
        IsSetupCompleted = true;
        return new UserSetupResult() { Username = Username, Language = SelectedLanguageName.LanguageName };
    }

}

public class UserSetupResult
{
    public string Username { get; set; }
    public string Language { get; set; }
}