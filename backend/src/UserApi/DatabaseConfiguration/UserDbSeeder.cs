// ---------------------------------------------------------------------
// UserDbSeeder.cs
// ---------------------------------------------------------------------
// This file contains the UserDbSeeder class, which is responsible for 
// seeding the UserDb with a default admin user upon application startup.
// 
// Usage:
// This class is invoked in the Program.cs file of the application and 
// is used only in development environments to populate the database 
// with essential data. It checks for the existence of specific user 
// records and creates them if they are not found, ensuring idempotency.
//
// Note: 
// Modify this class carefully, as changes can directly affect the initial 
// data state of the application. Ensure that any modifications are 
// thoroughly tested to maintain the integrity of the database seeding process.
// ---------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using UserApi.Entities;
using UserApi.Helpers;

namespace UserApi.DatabaseConfiguration
{
    public class UserDbSeeder
    {
        public static async Task SeedUserAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<UserDbContext>();
                    var passwordHasher = services.GetRequiredService<IPasswordHasher>();

                    (byte[] passwordHash, byte[] passwordSalt) = passwordHasher.HashPassword("password");
                    context.Database.Migrate();

                    if (await context.Users.FirstOrDefaultAsync(u => u.Username == "admin") == null)
                    {
                        await context.AddAsync(new User
                        {
                            Username = "admin",
                            Email = "admin@admin.com",
                            FirstName = "Adam",
                            LastName = "Admin",
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            Role = "Admin",
                            IsActive = true,
                            DateCreated = DateTime.Now
                        });
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<UserDbSeeder>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
