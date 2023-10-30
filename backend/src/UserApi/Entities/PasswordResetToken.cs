using System;
using System.Collections.Generic;

namespace UserApi.Entities;

public partial class PasswordResetToken
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public bool? IsUsed { get; set; }

    public virtual User? User { get; set; }
}
