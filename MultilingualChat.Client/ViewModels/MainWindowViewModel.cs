﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using HarfBuzzSharp;
using Microsoft.VisualBasic;
using MultilingualChat.Client.Services;
using MultilingualChat.Client.Views;

namespace MultilingualChat.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    
    private readonly SignalRService _signalRService;
    private HubConnection _connection;

    public string InputContent { get; set; }
    public string SelectedLanguageName { get; set; }
    public string Username { get; set; }

    public void Initialize(string username, string language)
    {
        Username = username;
        SelectedLanguageName = language;
    }

    public ObservableCollection<Language> LanguageList { get; set; }
    
    public ObservableCollection<Message> ChatMessages { get; set; } = new ObservableCollection<Message>();
    public MainWindowViewModel(SignalRService signalRService)
    {
        _signalRService = signalRService;
        _connection = _signalRService.GetConnection();
        LanguageList = new ObservableCollection<Language>(new List<Language>
        {
            new Language("Danish"),
            new Language("English"),

        });
        
        _connection.On<string, string>("SendMessage", (message, sender) =>
        {
            ChatMessages.Add(new Message
            {
                MessageText = message,
                MessageSender = sender,
                MessageTimestamp = DateTime.Now
            });
            Console.WriteLine("ON MESSAGE");
            Console.WriteLine(message);
        });


    }




    public new event PropertyChangedEventHandler? PropertyChanged;
    public int Counter { get; set; } = 0;
    
    
    
    public async void HandleButtonClick()
    {
        
        var connection = _signalRService.GetConnection();
        await connection.InvokeAsync("SendMessage", InputContent, "User1");
        // Counter += 1;
        // Console.WriteLine(Counter);
        // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
        InputContent = "";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputContent)));

        Console.WriteLine(SelectedLanguageName);
        
        

    }


}