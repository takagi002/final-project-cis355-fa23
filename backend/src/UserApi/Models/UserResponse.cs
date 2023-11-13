using System;

namespace UserApi.Models;

public class UserResponse
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? LastLogin { get; set; }
}