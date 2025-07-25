using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BlogPost
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? PostedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? PostedByNavigation { get; set; }
}
