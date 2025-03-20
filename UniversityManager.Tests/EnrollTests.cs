using System.Threading.Tasks;
using Xunit;
using UniversityManager.ViewModels;
using UniversityManager.Services;
using UniversityManager.Models;
using System.Collections.Generic;

public class StudentTests
{
    [Fact]
    public async Task EnrollStudent_Should_Add_Subject_To_EnrolledSubjects()
    {
        // Arrange
        var student = new Student { Name = "Test Student", EnrolledSubjects = new List<int>() };
        var subject = new Subject { Id = 1, Name = "Mathematics", StudentsEnrolled = new List<int>() };
        var dataService = new DataService();

        // Add the subject to the data service
        dataService.Data.Subjects.Add(subject);
        var studentVM = new StudentViewModel(student);

        // Act
        await studentVM.EnrollAsync(subject);

        // Assert
        Assert.Contains(studentVM.EnrolledSubjects, s => s.Id == subject.Id);
    }
}
