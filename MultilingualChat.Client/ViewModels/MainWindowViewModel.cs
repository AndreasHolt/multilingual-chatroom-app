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

    public string RoomId { get; set; }

    public ObservableCollection<Language> LanguageList { get; set; }

    public ObservableCollection<Message> ChatMessages { get; set; } = new ObservableCollection<Message>();

    public void CopyRoomIdToClipboard()
    {
        throw new NotImplementedException();
    }

    public void OnUserConfirmed(UserSetupResult result)
    {
        _connection = _signalRService.GetConnection();

        Username = result.Username;
        SelectedLanguageName = result.Language;
        RoomId = _signalRService.GetRoomId();


        _connection.On<string, string, string>("SendMessage", (message, sender, color) =>
        {
            if (message != "" && message != null)
            {
                ChatMessages.Add(new Message
                {
                    MessageText = message,
                    MessageSender = sender,
                    MessageSenderColor = color,
                    MessageTimestamp = DateTime.Now
                });
            }

            _connection.SendAsync("GetUsersInRoom");
        });
    }

    public MainWindowViewModel(SignalRService signalRService, UserSetupViewModel userSetupViewModel)
    {
        _signalRService = signalRService;
        userSetupViewModel.UserConfirmed += OnUserConfirmed;
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
    public int Counter { get; set; } = 0;

    public async void HandleButtonClick()
    {
        var connection = _signalRService.GetConnection();
        await connection.InvokeAsync("SendMessage", InputContent);
        InputContent = "";
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputContent)));
    }
}