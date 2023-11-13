using Microsoft.AspNetCore.Mvc;
using UserApi.Authorization;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public Task<IActionResult> GetProfile()
    {
        throw new NotImplementedException();
    }

    // Add more actions here
}
