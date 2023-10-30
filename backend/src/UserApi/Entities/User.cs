using System;
using System.Collections.Generic;

namespace UserApi.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public DateTime? LastLogin { get; set; }

    public int? FailedLoginAttempts { get; set; }
}
