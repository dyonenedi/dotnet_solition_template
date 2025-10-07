using System;
using System.Collections.Generic;

namespace app.api.Application.Models;

public partial class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual User User { get; set; } = null!;
}
