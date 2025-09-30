using System;
using System.Collections.Generic;

namespace app.auth.Application.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
