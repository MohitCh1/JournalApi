using JournalApi.Data;
using JournalApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodController : ControllerBase
    {
        private readonly JournalDbContext journalentry;

        public MoodController(JournalDbContext journalentry)
        {
            this.journalentry = journalentry;
        }

        [HttpGet("analytics/mood-tracker")]
        public IActionResult Get() {
            var userName = User.Identity.Name;
            var user = journalentry.Users.FirstOrDefault(x => x.Username == userName);
            if (user == null)
            {
                return Unauthorized("NO user found");
            }
            var result = journalentry.JournalEntries
                       .Where(x => x.UserId == user.Id)
                       .GroupBy(x => x.Mood)
                       .Select(g => new
                       {
                           Mood = g.Key,
                           count = g.Count()

                       }).ToList();
          return Ok(result);
        
        }

        [HttpGet("analytics/range")]
        public IActionResult GetByRange(DateTime beginDate, DateTime endDate) {
            var userName = User.Identity.Name;
            var user = journalentry.Users.FirstOrDefault(x => x.Username == userName);
            if (user == null) return Unauthorized("No User Found");

            endDate = endDate.Date.AddDays(1).AddTicks(-1);
            beginDate = beginDate.Date;
            var result = journalentry.JournalEntries
                        .Where(x => x.UserId == user.Id &&  x.CreatedAt.HasValue
             && x.CreatedAt.Value >= beginDate
             && x.CreatedAt.Value <= endDate)
                        .GroupBy(x => x.CreatedAt.Value.Date)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Count(),
                            MostMood = g.GroupBy(x => x.Mood)
                                     .OrderByDescending(m => m.Count())
                                     .Select(m => m.Key)
                                     .FirstOrDefault()

                        }).OrderBy(x => x.Date).ToList();
                 return Ok(result);     
                       
        }




       


       
    }
}
