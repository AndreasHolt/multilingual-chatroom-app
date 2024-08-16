using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using HarfBuzzSharp;
using MultilingualChat.Client.Services;

namespace MultilingualChat.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly SignalRService _signalRService;

    public ObservableCollection<Language> LanguageList { get; set; }
    public MainWindowViewModel(SignalRService signalRService)
    {
        _signalRService = signalRService;
        LanguageList = new ObservableCollection<Language>(new List<Language>
        {
            new Language("Danish"),
            new Language("English"),

        });

    }




    public new event PropertyChangedEventHandler? PropertyChanged;
    public int Counter { get; set; } = 0;
    
    
    
    public async void HandleButtonClick()
    {
        var connection = _signalRService.GetConnection();
        await connection.InvokeAsync("SendMessage", "Test message", "Test username");
        Counter += 1;
        Console.WriteLine(Counter);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
        
        
    }


}