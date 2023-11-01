using AutoMapper;
using UserApi.Authorization;
using UserApi.Entities;
using UserApi.Helpers;
using UserApi.Models;
using UserApi.Repositories;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;


    public UserService(IUserRepository userRepository, IJwtUtils jwtUtils, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
    {
        // get user from database
        var user = await _userRepository.GetUserByUsernameAsync(model.Username);

        // return null if user not found
        if (user == null) return null;

        // check if the provided password matches the password in the database and return null if it doesn't
        if (!_passwordHasher.ValidatePassword(model.Password, user.PasswordHash, user.PasswordSalt)) return null;

        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);

        
        // map user and token to response model with Automapper and return
        return _mapper.Map<AuthenticateResponse>(user, opts => opts.Items["Token"] = token);
    }

    public async Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest userRequest)
    {
        // Hash and salt the password
        (byte[] passwordHash, byte[] passwordSalt) = _passwordHasher.HashPassword(userRequest.Password);

        // Map CreateUserRequest model to User entity with Automapper
        var userEntity = _mapper.Map<User>(userRequest);

        // Assign hashed and salted password to user entity
        userEntity.PasswordHash = passwordHash;
        userEntity.PasswordSalt = passwordSalt;

        // Create user in database
        var createdUser = await _userRepository.CreateUserAsync(userEntity)
            ?? throw new Exception("An error occurred when creating user. Try again later.");

        // Map User entity to CreateUserResponse model with Automapper
        var userResponse = _mapper.Map<CreateUserResponse>(createdUser);
        return userResponse;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }
}
