using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MultilingualChat.Client.Services;
using MultilingualChat.Client.ViewModels;
using MultilingualChat.Client.Views;

namespace MultilingualChat.Client;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddTransient<MainWindowViewModel>();
        
        services.AddSingleton<SignalRService>(); // Here we register SignalRService as a singleton
        var serviceProvider = services.BuildServiceProvider();

        var vm = serviceProvider.GetRequiredService<MainWindowViewModel>();
       
        
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Set up mainWindow which should appear after dialogue
            var mainWindow = new MainWindow
            {
                DataContext = vm
            };
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
           
            // Set up the user setup dialogue
            var userSetupWindow = new UserSetupWindow();
            var result = await userSetupWindow.ShowDialog<bool>(desktop.MainWindow);

            if (result)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                
                
                
            }
        }




        base.OnFrameworkInitializationCompleted();
        
        // Here we start the SignalR connection
        // Start the connection after the base call to avoid blocking the main thread with SignalR.
        // Otherwise GUI doesn't open
        var signalRService = serviceProvider.GetRequiredService<SignalRService>();
        await signalRService.StartConnectionAsync(); 
    }
}