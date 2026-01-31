using JournalApi.DTO;
using JournalApi.Models;

namespace JournalApi.Services.Interface
{
    public interface IJournalService
    {
        List<JournalEntry> GetJournalEntries(string username);
        JournalEntry GetJournalEntryByID(int id, string username);
        JournalEntry CreateJournalEntry(JournalEntryDTO dto, string username);
        JournalEntry UpdateJournalEntry(int id, string username, JournalEntryDTO dto);
        JournalEntry DeleteJournalEntry(int id, string username);
        
    }
}
