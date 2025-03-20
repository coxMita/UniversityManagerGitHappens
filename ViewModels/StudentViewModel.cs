using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;
using UniversityManager.Services;
using System.Threading.Tasks;

namespace UniversityManager.ViewModels
{
    public partial class StudentViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly Student _currentStudent;

        public ObservableCollection<Subject> AvailableSubjects { get; set; }
        public ObservableCollection<Subject> EnrolledSubjects { get; set; }

        private Subject _selectedAvailableSubject;
        public Subject SelectedAvailableSubject
        {
            get => _selectedAvailableSubject;
            set
            {
                if (SetProperty(ref _selectedAvailableSubject, value))
                {
                    OnPropertyChanged(nameof(SelectedSubject));
                    OnPropertyChanged(nameof(SelectedSubjectTeacherName));
                }
            }
        }

        private Subject _selectedEnrolledSubject;
        public Subject SelectedEnrolledSubject
        {
            get => _selectedEnrolledSubject;
            set
            {
                if (SetProperty(ref _selectedEnrolledSubject, value))
                {
                    OnPropertyChanged(nameof(SelectedSubject));
                    OnPropertyChanged(nameof(SelectedSubjectTeacherName));
                }
            }
        }

        public Subject SelectedSubject => SelectedAvailableSubject ?? SelectedEnrolledSubject;

        // For displaying teacher name of the selected subject
        public string SelectedSubjectTeacherName
        {
            get
            {
                if (SelectedSubject == null)
                    return "";
                var teacher = _dataService.Data.Teachers
                    .FirstOrDefault(t => t.Id == SelectedSubject.TeacherId);
                return teacher?.Name ?? "";
            }
        }

        [ObservableProperty]
        private string searchTerm = "";

        partial void OnSearchTermChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(FilteredAvailableSubjects));
        }

        public ObservableCollection<Subject> FilteredAvailableSubjects
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                    return AvailableSubjects;
                else
                {
                    var filtered = AvailableSubjects
                        .Where(s => s.Name.IndexOf(SearchTerm, System.StringComparison.OrdinalIgnoreCase) >= 0);
                    return new ObservableCollection<Subject>(filtered);
                }
            }
        }

        public ICommand EnrollCommand { get; }
        public ICommand DropCommand { get; }

        public StudentViewModel(Student student)
        {
            _currentStudent = student;
            _dataService = new DataService();
            _dataService.LoadData();

            RefreshSubjects();

            EnrollCommand = new AsyncRelayCommand<Subject>(EnrollAsync);
            DropCommand = new AsyncRelayCommand<Subject>(DropAsync);
        }

        private void RefreshSubjects()
        {
            // Subjects not yet enrolled by the student.
            var available = _dataService.Data.Subjects
                .Where(s => !_currentStudent.EnrolledSubjects.Contains(s.Id))
                .ToList();
            AvailableSubjects = new ObservableCollection<Subject>(available);

            // Subjects the student is enrolled in.
            var enrolled = _dataService.Data.Subjects
                .Where(s => _currentStudent.EnrolledSubjects.Contains(s.Id))
                .ToList();
            EnrolledSubjects = new ObservableCollection<Subject>(enrolled);

            OnPropertyChanged(nameof(AvailableSubjects));
            OnPropertyChanged(nameof(EnrolledSubjects));
            OnPropertyChanged(nameof(FilteredAvailableSubjects));
        }
        [ObservableProperty]
        private string notificationMessage = "";

        public async Task EnrollAsync(Subject subject)
        {
            if (subject == null)
                return;

            if (!_currentStudent.EnrolledSubjects.Contains(subject.Id))
            {
                _currentStudent.EnrolledSubjects.Add(subject.Id);
            }
            if (!subject.StudentsEnrolled.Contains(_currentStudent.Id))
            {
                subject.StudentsEnrolled.Add(_currentStudent.Id);
            }
            _dataService.SaveData();

            RefreshSubjects();
            // Set notification message
            NotificationMessage = $"Successfully enrolled in {subject.Name}!";
            // Wait 3 seconds
            await Task.Delay(3000);
            NotificationMessage = "";
        }

        private async Task DropAsync(Subject subject)
        {
            if (subject == null)
                return;

            _currentStudent.EnrolledSubjects.Remove(subject.Id);
            subject.StudentsEnrolled.Remove(_currentStudent.Id);
            _dataService.SaveData();

            RefreshSubjects();
            // Set notification message
            NotificationMessage = $"Successfully removed from {subject.Name}!";
            await Task.Delay(3000);
            NotificationMessage = "";
        }
    }
}
