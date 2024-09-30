using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using MultilingualChat.Client.Services;

namespace MultilingualChat.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly SignalRService _signalRService;
    private HubConnection _connection;
    public string InputContent { get; set; }
    public string Username { get; set; }
    public string SelectedLanguageName { get; set; }

    public ObservableCollection<Language> LanguageList { get; set; }

    public ObservableCollection<Message> ChatMessages { get; set; } = new ObservableCollection<Message>();

    public void OnUserConfirmed(UserSetupResult result)
    {
        Username = result.Username;
        SelectedLanguageName = result.Language;

        _connection = _signalRService.GetConnection();
        Console.WriteLine("CLIENT: Connection created to id " + _connection.ConnectionId);

        _connection.On<string, string>("SendMessage", (message, sender) =>
        {
            Console.WriteLine("MESSAGE RECEIVED ON CLIENT SIDE");
            if (message != "" && message != null)
            {
                ChatMessages.Add(new Message
                {
                    MessageText = message,
                    MessageSender = sender,
                    MessageTimestamp = DateTime.Now
                });
            }

            Console.WriteLine("ON MESSAGE");
            Console.WriteLine(message);
            _connection.SendAsync("GetAllUsers");
        });
    }

    public MainWindowViewModel(SignalRService signalRService, UserSetupViewModel userSetupViewModel)
    {
        _signalRService = signalRService;
        userSetupViewModel.UserConfirmed += OnUserConfirmed;


        // We add a connection to the 
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
    public int Counter { get; set; } = 0;

    public async void HandleButtonClick()
    {
        var connection = _signalRService.GetConnection();
        await connection.InvokeAsync("SendMessage", InputContent, Username);
        InputContent = "";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputContent)));

        Console.WriteLine(SelectedLanguageName);
    }
}