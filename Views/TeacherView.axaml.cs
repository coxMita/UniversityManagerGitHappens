using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace UniversityManager.Views
{
    public partial class TeacherView : Window
    {
        public TeacherView()
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

        // Handle logout
        private void OnLogoutButtonClick(object? sender, RoutedEventArgs e)
        {
            // Open a new LoginView
            var loginView = new LoginView();
            loginView.Show();
            // Close this window
            this.Close();
        }
    }
}
