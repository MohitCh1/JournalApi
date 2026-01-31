using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalApi.Models;

public partial class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public string Roles { get; set; } = null!;

    public int currentStreak { get; set; } = 0;
    public int LongestStreak { get; set; } = 0;
    
    public DateTime ?LastJournalDate { get; set; }

    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
