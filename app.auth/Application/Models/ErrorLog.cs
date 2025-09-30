using System;
using System.Collections.Generic;

namespace app.auth.Application.Models;

public partial class ErrorLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Source { get; set; } = null!;

    public string ExceptionType { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string Message { get; set; } = null!;

    public string StackTrace { get; set; } = null!;
}
