using UserApi.Authorization;
using UserApi.Helpers;

namespace UserApi.Extensions;

public static class JwtExtensions
{
    public static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        // Register JWT utilities and services
        builder.Services.AddScoped<IJwtUtils, JwtUtils>();
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    }
}
