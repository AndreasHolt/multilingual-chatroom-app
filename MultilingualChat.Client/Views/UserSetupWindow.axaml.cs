using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MultilingualChat.Client.ViewModels;

namespace MultilingualChat.Client.Views;

public partial class UserSetupWindow : Window
{
    private UserSetupViewModel _view;
    public UserSetupWindow()
    {
        InitializeComponent();
    }
    

    public void CloseWithResult(UserSetupResult result)
    {
        Console.WriteLine("Result is " + result.Username);
        result = _view.StartChat();
        Close(result);
    }
}