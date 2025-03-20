using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using UniversityManager.ViewModels;
using UniversityManager.Views;

namespace UniversityManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

   public override void OnFrameworkInitializationCompleted()
{

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        // Start the application with the LoginView
        desktop.MainWindow = new LoginView();
    }

    base.OnFrameworkInitializationCompleted();
}


    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
    
}