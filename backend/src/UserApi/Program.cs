using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using UserApi.Authorization;
using UserApi.Extensions;
using UserApi.Mappings;
using UserApi.DatabaseConfiguration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = "Google";
    })
    .AddOAuth("Google", options =>
    {
        options.ClientId = Env.GetString("GOOGLE_CLIENT_ID");
        options.ClientSecret = Env.GetString("GOOGLE_CLIENT_SECRET");
        options.CallbackPath = new PathString("/signin-google");
        options.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";
        options.TokenEndpoint = "https://accounts.google.com/o/oauth2/token";
        options.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                // map to database
                return Task.CompletedTask;
            }
        };
    });
    
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger with bearer token authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "User API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.ConfigureDatabase();
builder.ConfigureJwt();

// Configure logging
builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddDebug();

var app = builder.Build();

// Add a user to the database during the startup process only when running locally
if (app.Environment.IsDevelopment())
{
    // Create default admin user if it doesn't exist
    await UserDbSeeder.SeedUserAsync(app.Services);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
