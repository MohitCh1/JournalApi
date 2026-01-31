namespace JournalApi.DTO
{
    public class JournalEntryDTO
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Mood { get; set; } = "Neutral";
    }
}
