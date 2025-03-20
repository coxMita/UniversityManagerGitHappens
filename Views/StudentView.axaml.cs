using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UniversityManager.Views
{
    public partial class StudentView : Window
    {
        public StudentView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void OnLogoutButtonClick(object? sender, RoutedEventArgs e)
        {
            // Open LoginView and close StudentView
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
