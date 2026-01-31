using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalApi.Models;

public partial class JournalEntry
{
    public int Id { get; set; }
    
    public string? Title { get; set; }

    public string? Content { get; set; }

    public string ? Mood { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
