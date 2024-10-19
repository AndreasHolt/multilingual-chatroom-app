using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Classic.Avalonia.Theme;
using MultilingualChat.Client.ViewModels;

namespace MultilingualChat.Client.Views;

public partial class UserSetupWindow : ClassicWindow
{
    private UserSetupViewModel _view;

    public UserSetupWindow()
    {
        InitializeComponent();
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        // If the user is joining a room, they must enter a room id
        if (JoinRoomRadioButton.IsChecked == true && string.IsNullOrEmpty(RoomIdInput.Text))
        {
            return;
        }

        // User must enter a username and a language
        if (!string.IsNullOrEmpty(UsernameInput.Text) && LanguageAutoCompleteBox.SelectedItem != null)
        {
            Close(true);
        }
    }

    public void CloseWithResult(UserSetupResult result)
    {
        Console.WriteLine("Result is " + result.Username);
        result = _view.StartChat();
        Close(result);
    }
}