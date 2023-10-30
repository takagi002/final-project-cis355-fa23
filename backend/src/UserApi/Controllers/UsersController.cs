using Microsoft.AspNetCore.Mvc;
using UserApi.Authorization;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Authorize(Role = RoleNames.Admin)]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAllAsync();
        return Ok(users);
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest newUser)
    {
        var createdUser = await _userService.CreateUserAsync(newUser);
        return Ok(createdUser);
    }
}
