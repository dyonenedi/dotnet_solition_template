using System;
using System.Collections.Generic;

namespace app.api.Application.Models;

public partial class PostLike
{
    public int Id { get; set; }

    public int LikerId { get; set; }

    public int PostId { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public bool? Active { get; set; }

    public virtual User Liker { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
