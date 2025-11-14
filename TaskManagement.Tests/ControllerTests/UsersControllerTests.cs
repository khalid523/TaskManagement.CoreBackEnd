using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Interfaces;
using TaskManagement.API.Controllers;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Tests.ControllerTests;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<UsersController>> _mockLogger;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<UsersController>>();
        _controller = new UsersController(_mockUserService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithUsersList()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "User 1", Email = "user1@example.com", Role = "Admin" },
            new User { Id = 2, Name = "User 2", Email = "user2@example.com", Role = "User" }
        };
        _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count());
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var userId = 1;
        var user = new User { Id = userId, Name = "Test User", Email = "test@example.com", Role = "User" };
        _mockUserService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = 999;
        _mockUserService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _controller.GetById(userId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var request = new CreateUserRequest { Name = "New User", Email = "new@example.com", Role = "User" };
        var createdUser = new User { Id = 1, Name = request.Name, Email = request.Email, Role = request.Role };
        _mockUserService.Setup(s => s.CreateUserAsync(request.Name, request.Email, request.Role))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(UsersController.GetById), createdResult.ActionName);
        Assert.Equal(createdUser.Id, ((User)createdResult.Value!).Id);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var userId = 1;
        _mockUserService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}