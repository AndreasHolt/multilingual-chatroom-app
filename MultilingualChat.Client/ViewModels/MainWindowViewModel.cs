using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MultilingualChat.Client.Services;

namespace MultilingualChat.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly SignalRService _signalRService;

    public MainWindowViewModel(SignalRService signalRService)
    {
        _signalRService = signalRService;

    }
    
    
    
    
    
    public new event PropertyChangedEventHandler? PropertyChanged;
    public int Counter { get; set; } = 0;
    
    
    
    public async void HandleButtonClick()
    {
        await _signalRService.StopConnectionAsync();
        Counter += 1;
        Console.WriteLine(Counter);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Counter)));
        
        
    }


}