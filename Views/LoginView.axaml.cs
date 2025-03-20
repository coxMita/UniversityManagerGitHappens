using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using UniversityManager.ViewModels;
using UniversityManager.Models;
using UniversityManager.Services;
using System;

namespace UniversityManager.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();

#if DEBUG
            this.AttachDevTools();
#endif

            if (DataContext is LoginViewModel vm)
            {
                vm.StudentLoggedIn += OnStudentLoggedIn;
                vm.TeacherLoggedIn += OnTeacherLoggedIn;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnStudentLoggedIn(Student student)
        {
            var studentViewModel = new StudentViewModel(student);
            var studentView = new StudentView
            {
                DataContext = studentViewModel
            };
            studentView.Show();
            this.Close();
        }

        private void OnTeacherLoggedIn(Teacher teacher)
        {
            var teacherViewModel = new TeacherViewModel(teacher);
            var teacherView = new TeacherView
            {
                DataContext = teacherViewModel
            };
            teacherView.Show();
            this.Close();
        }

        // When the "Show" CheckBox is checked, remove the password mask and show yeti2.png.
        private void OnShowPasswordChecked(object? sender, RoutedEventArgs e)
        {
            var passwordTextBox = this.FindControl<TextBox>("PasswordBox");
            if (passwordTextBox != null)
            {
                passwordTextBox.PasswordChar = '\0';
            }

            // image visibility
            var logoImage1 = this.FindControl<Image>("LogoImage1");
            var logoImage2 = this.FindControl<Image>("LogoImage2");

            if (logoImage1 != null && logoImage2 != null)
            {
                logoImage1.IsVisible = false;
                logoImage2.IsVisible = true;
            }
        }

        // When unchecked, reapply the mask and show yeti1.png.
        private void OnShowPasswordUnchecked(object? sender, RoutedEventArgs e)
        {
            var passwordTextBox = this.FindControl<TextBox>("PasswordBox");
            if (passwordTextBox != null)
            {
                passwordTextBox.PasswordChar = '*';
            }

            var logoImage1 = this.FindControl<Image>("LogoImage1");
            var logoImage2 = this.FindControl<Image>("LogoImage2");

            if (logoImage1 != null && logoImage2 != null)
            {
                logoImage1.IsVisible = true;
                logoImage2.IsVisible = false;
            }
        }
    }
}
