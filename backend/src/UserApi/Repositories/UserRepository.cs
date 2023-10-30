using Microsoft.EntityFrameworkCore;
using UserApi.Entities;
using UserApi.Exceptions;

namespace UserApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> CreateUserAsync(User user)
    {
        try
        {
            // Add user to database
            var createdUser = (await _context.Users.AddAsync(user)).Entity;
            // Save changes to database
            await _context.SaveChangesAsync();

            if (createdUser == null)
            {
                _logger.LogError("An error occurred while creating user. User data: {User}", user);
                return null;
            }

            return createdUser;
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
        {
            _logger.LogWarning("Attempted to create a user with a duplicate email. Email: {Email}", user.Email);
            throw new DuplicateEmailException("This email is already in use.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the entity changes.");
            throw new UserCreationFailedException("An unexpected error occurred while creating the user.", ex);
        }
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    // ... Add other methods here as needed
}