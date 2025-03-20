using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;
using UniversityManager.Services;

namespace UniversityManager.ViewModels
{
    public partial class TeacherViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        private readonly Teacher _currentTeacher;

        public ObservableCollection<Subject> MySubjects { get; set; }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (SetProperty(ref _selectedSubject, value))
                {
                    OnPropertyChanged(nameof(SelectedSubjectTeacherName));
                }
            }
        }

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
        private string newSubjectName = "";

        [ObservableProperty]
        private string newSubjectDescription = "";

        [ObservableProperty]
        private string searchTerm = "";

        partial void OnSearchTermChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(FilteredMySubjects));
        }

        //filtered subjects based on the search term.
        public ObservableCollection<Subject> FilteredMySubjects
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                    return MySubjects;
                else
                {
                    var filtered = MySubjects
                        .Where(s => s.Name.IndexOf(SearchTerm, System.StringComparison.OrdinalIgnoreCase) >= 0);
                    return new ObservableCollection<Subject>(filtered);
                }
            }
        }

        public ICommand CreateSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }

        public TeacherViewModel(Teacher teacher)
        {
            _currentTeacher = teacher;
            _dataService = new DataService();
            _dataService.LoadData();

            CreateSubjectCommand = new RelayCommand(CreateSubject);
            DeleteSubjectCommand = new RelayCommand<Subject>(DeleteSubject);

            RefreshMySubjects();
        }

        private void RefreshMySubjects()
        {
            var teacherSubjects = _dataService.Data.Subjects
                .Where(s => s.TeacherId == _currentTeacher.Id)
                .ToList();

            MySubjects = new ObservableCollection<Subject>(teacherSubjects);
            OnPropertyChanged(nameof(MySubjects));
            OnPropertyChanged(nameof(FilteredMySubjects));
        }

        private void CreateSubject()
        {
            if (string.IsNullOrWhiteSpace(NewSubjectName))
                return;

            var newId = _dataService.Data.Subjects.Count + 1;
            var subject = new Subject
            {
                Id = newId,
                Name = NewSubjectName,
                Description = NewSubjectDescription,
                TeacherId = _currentTeacher.Id,
                // Automatically set TeacherName from the logged-in teacher
                TeacherName = _currentTeacher.Name
            };

            _dataService.Data.Subjects.Add(subject);
            _currentTeacher.Subjects.Add(subject.Id);
            _dataService.SaveData();

            NewSubjectName = "";
            NewSubjectDescription = "";

            RefreshMySubjects();
        }

        private void DeleteSubject(Subject subjectToDelete)
        {
            if (subjectToDelete == null)
                return;

            _dataService.Data.Subjects.Remove(subjectToDelete);
            _currentTeacher.Subjects.Remove(subjectToDelete.Id);

            // Also remove this subject from any enrolled students
            foreach (var student in _dataService.Data.Students)
            {
                student.EnrolledSubjects.Remove(subjectToDelete.Id);
            }

            _dataService.SaveData();
            RefreshMySubjects();
        }
    }
}
