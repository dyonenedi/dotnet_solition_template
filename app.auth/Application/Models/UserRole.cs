using System;
using System.Collections.Generic;

namespace app.auth.Application.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public DateTime InsertDate { get; set; }

    public DateTime UptadeDate { get; set; }

    public virtual User User { get; set; } = null!;
}
