namespace UserApi.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a request to authenticate a user.
/// </summary>
public class AuthenticateRequest
{
    /// <summary>
    /// The username of the user to authenticate.
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// The password of the user to authenticate.
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;
}