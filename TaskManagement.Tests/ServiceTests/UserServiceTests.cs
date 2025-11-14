using Moq;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Interfaces;
using TaskManagement.API.Services;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Tests.ServiceTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, Name = "Test User", Email = "test@example.com", Role = "User" };
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("Test User", result.Name);
        _mockUserRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var userId = 999;
        _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateUserAsync_WithValidData_CreatesUser()
    {
        // Arrange
        var name = "New User";
        var email = "newuser@example.com";
        var role = "User";
        var createdUser = new User { Id = 1, Name = name, Email = email, Role = role };
        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        // Act
        var result = await _userService.CreateUserAsync(name, email, role);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(email, result.Email);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WithInvalidRole_ThrowsException()
    {
        // Arrange
        var name = "Test User";
        var email = "test@example.com";
        var invalidRole = "InvalidRole";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.CreateUserAsync(name, email, invalidRole));
    }

    [Fact]
    public async Task CreateUserAsync_WithEmptyName_ThrowsException()
    {
        // Arrange
        var name = "";
        var email = "test@example.com";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.CreateUserAsync(name, email, "User"));
    }

    [Fact]
    public async Task UpdateUserAsync_WithValidId_UpdatesUser()
    {
        // Arrange
        var userId = 1;
        var existingUser = new User { Id = userId, Name = "Old Name", Email = "old@example.com", Role = "User" };
        var updatedUser = new User { Id = userId, Name = "New Name", Email = "new@example.com", Role = "User" };

        _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
        _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _userService.UpdateUserAsync(userId, "New Name", "new@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("new@example.com", result.Email);
    }

    [Fact]
    public async Task DeleteUserAsync_WithValidId_DeletesUser()
    {
        // Arrange
        var userId = 1;
        _mockUserRepository.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
        _mockUserRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
    }
}