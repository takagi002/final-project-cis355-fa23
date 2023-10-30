using System.Threading.Tasks;
using AutoMapper;
using Moq;
using UserApi.Authorization;
using UserApi.Entities;
using UserApi.Helpers;
using UserApi.Models;
using UserApi.Repositories;
using UserApi.Services;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    private readonly Mock<IJwtUtils> _mockJwtUtils = new Mock<IJwtUtils>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
    private readonly Mock<IPasswordHasher> _mockPasswordHasher = new Mock<IPasswordHasher>();
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(
            _mockUserRepository.Object, 
            _mockJwtUtils.Object, 
            _mockMapper.Object, 
            _mockPasswordHasher.Object
        );
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var loginRequest = new AuthenticateRequest { Username = "testuser", Password = "testpass" };
        _mockUserRepository.Setup(x => x.GetUserByUsernameAsync("testuser")).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.Authenticate(loginRequest);

        // Assert
        Assert.Null(result);
    }

    // Add more tests here for other scenarios
}
