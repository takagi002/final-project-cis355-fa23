using UserApi.Entities;
using UserApi.Models;

namespace UserApi.Services;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest user);
}