using Google.GenAI.Types;
using JournalApi.Data;
using JournalApi.DTO;
using JournalApi.Models;
using JournalApi.Services.Interface;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace JournalApi.Services
{
    public class JournalEntryService: IJournalService
    {
        private readonly JournalDbContext context;

        public JournalEntryService(JournalDbContext context)
        {
            this.context = context;
        }

        public List<JournalEntry> GetJournalEntries(string username)
        {
            var user = context.Users.FirstOrDefault(x => x.Username == username)
                ?? throw new UnauthorizedAccessException();

            return context.JournalEntries.Where(x => x.UserId == user.Id).ToList();


            
        }

        public JournalEntry GetJournalEntryByID(int id, string username)
        {
            var user=context.Users.FirstOrDefault(x=>x.Username == username)
                ?? throw new UnauthorizedAccessException();

            var entry=context.JournalEntries.FirstOrDefault(x=>x.UserId == user.Id && x.Id==id);
            if (entry == null) throw new KeyNotFoundException("entry not found");

            return entry;
        }

        public JournalEntry CreateJournalEntry(JournalEntryDTO dto, string username)
        {
            var user=context.Users.FirstOrDefault(x=>x.Username==username)
                ?? throw new UnauthorizedAccessException();

            var entry = new JournalEntry
            {
                Title=dto.Title,
                Content=dto.Content,
                Mood=dto.Mood ?? "Neutral",
                CreatedAt = DateTime.Now,
                UserId = user.Id

            };
           
            context.JournalEntries.Add(entry);

            if (user.LastJournalDate == null) user.currentStreak = 1;
            else
            {
                var today = DateTime.Now;
                var diff=(today-user.LastJournalDate.Value.Date).Days;
                user.currentStreak = diff == 1 ? user.currentStreak + 1 : 0;
            }
            user.LastJournalDate = DateTime.Now;
            user.LongestStreak = Math.Max(user.LongestStreak, user.currentStreak);

            context.SaveChanges();


            return entry;



        }

        public JournalEntry UpdateJournalEntry(int id, string username, JournalEntryDTO dto)
        {
            var user=context.Users.FirstOrDefault(x=>x.Username== username) ?? throw new UnauthorizedAccessException();
            var entry = context.JournalEntries.FirstOrDefault(x => x.UserId == user.Id && x.Id == id)
                ?? throw new KeyNotFoundException("no record found");
            entry.Title = dto.Title;
            entry.Content = dto.Content;
            entry.Mood = dto.Mood ?? entry.Mood;
            entry.UpdatedAt=DateTime.Now;
            context.SaveChanges();
            return entry;
        }

        public JournalEntry DeleteJournalEntry(int id, string username)
        {
            var user = context.Users.FirstOrDefault(x => x.Username == username) ?? throw new UnauthorizedAccessException();
            var entry = context.JournalEntries.FirstOrDefault(x => x.Id == id && x.UserId == user.Id) ?? throw new KeyNotFoundException("not found data");

            context.JournalEntries.Remove(entry);
            context.SaveChanges();
            return entry;
        }
    }
}
