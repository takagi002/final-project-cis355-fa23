namespace UserApi.Models;

using UserApi.Entities;

/// <summary>
/// Represents the response returned by the authentication endpoint.
/// </summary>
public class AuthenticateResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;
}