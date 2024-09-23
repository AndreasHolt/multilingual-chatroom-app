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
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _view = new UserSetupViewModel();
        this.Content = _view;
    }
}