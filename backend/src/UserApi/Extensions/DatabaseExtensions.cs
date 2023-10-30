using Microsoft.EntityFrameworkCore;
using UserApi.Entities;

namespace UserApi.Extensions;

public static class DatabaseExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        // Get the connection string from the appsettings.json file.
        string? userDbConnectionString = builder.Configuration.GetConnectionString("UserDb");

        // throw an exception if the connection string is null or empty.
        if (string.IsNullOrEmpty(userDbConnectionString))
        {
            throw new InvalidOperationException("UserDb connection string is null or empty.");
        }

        // Register the DbContext with the DI container.
        builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(userDbConnectionString),
            ServiceLifetime.Scoped
        );
    }
}

