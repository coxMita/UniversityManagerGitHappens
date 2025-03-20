using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;
using UniversityManager.Services;

namespace UniversityManager.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private string username = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string selectedRole = "Student";

        public List<string> Roles { get; } = new() { "Student", "Teacher" };

        // The command for the Login button
        public ICommand LoginCommand { get; }

        public event Action<Student>? StudentLoggedIn;
        public event Action<Teacher>? TeacherLoggedIn;

        public LoginViewModel()
        {
            _dataService = new DataService();
            _dataService.LoadData();

            // If no students exist, add a default student.
            if (!_dataService.Data.Students.Any())
            {
                _dataService.Data.Students.Add(new Student
                {
                    Id = 1,
                    Name = "John Student",
                    Username = "student1",
                    Password = "pass1"
                });
            }
            // If no teachers exist, add a default teacher.
            if (!_dataService.Data.Teachers.Any())
            {
                _dataService.Data.Teachers.Add(new Teacher
                {
                    Id = 1,
                    Name = "Jane Teacher",
                    Username = "teacher1",
                    Password = "pass1"
                });
            }
            _dataService.SaveData();

            // Initialize the Login command
            LoginCommand = new RelayCommand(OnLogin);
        }

        private void OnLogin()
        {
            // Retrieve the persisted student or teacher from the DataService.
            // This ensures that if any enrollment changes were made in a previous session,
            // they are preserved and loaded here.
            var student = _dataService.Data.Students
                .FirstOrDefault(s => s.Username == Username && s.Password == Password);
            var teacher = _dataService.Data.Teachers
                .FirstOrDefault(t => t.Username == Username && t.Password == Password);

            if (SelectedRole == "Student" && student != null)
            {
                StudentLoggedIn?.Invoke(student);
            }
            else if (SelectedRole == "Teacher" && teacher != null)
            {
                TeacherLoggedIn?.Invoke(teacher);
            }
            else
            {
                
            }
        }

        public bool AttemptLogin()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            if (SelectedRole == "Student")
            {
                return _dataService.Data.Students
                    .Any(s => s.Username == Username && s.Password == Password);
            }
            else if (SelectedRole == "Teacher")
            {
                return _dataService.Data.Teachers
                    .Any(t => t.Username == Username && t.Password == Password);
            }

            return false;
        }

    }
}
