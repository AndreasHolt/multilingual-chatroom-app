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
    private HubConnection _connection;

    public string InputContent { get; set; }
    
    

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
        Console.WriteLine("ONE ITERATION");

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

    }


}