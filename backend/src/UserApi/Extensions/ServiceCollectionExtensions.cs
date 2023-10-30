using UserApi.Authorization;
using UserApi.Helpers;
using UserApi.Repositories;
using UserApi.Services;

namespace UserApi.Extensions;

/// <summary>
/// ServiceCollectionExtensions contains extension methods for IServiceCollection,
/// providing a centralized way to register application-specific services and their dependencies.
///
/// This class follows the Dependency Injection (DI) pattern to promote loosely coupled code.
/// By adding services to the IServiceCollection, they can be injected into controllers or other services at runtime,
/// allowing for more modular and testable code.
///
/// To add a new service:
/// 1. Define the service interface and implementation.
/// 2. Add a new services.AddScoped, services.AddTransient, or services.AddSingleton call here,
///    depending on the desired service lifetime (Scoped, Transient, or Singleton).
/// 3. Specify the service interface and implementation as generic type parameters.
///
/// Once services are registered here, they can be injected into constructors throughout the application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application-specific services to the IServiceCollection.
    /// This method is called in the Program.cs file during application configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtUtils, JwtUtils>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        // Add more services as needed
    }
}
