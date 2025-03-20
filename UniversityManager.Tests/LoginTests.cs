using Xunit;  // Make sure xUnit is imported
using UniversityManager.ViewModels;

public class LoginTests
{
    [Fact]
    public void Valid_Student_Credentials_Should_Login_Successfully()
    {
        // Arrange
        var loginVM = new LoginViewModel();
        loginVM.Username = "student1"; // Ensure this exists in DataService
        loginVM.Password = "pass1"; // Ensure this is correct
        loginVM.SelectedRole = "Student";

        // Act
        bool loginSuccessful = loginVM.AttemptLogin();

        // Assert
        Assert.True(loginSuccessful, "Login should have been successful but returned false.");
    }
}
