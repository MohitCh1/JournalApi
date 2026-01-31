using JournalApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly GeminiServices service;

        public GeminiController(GeminiServices service)
        {
            this.service = service;
        }


        [HttpPost("ai-suggestion")]
        public async Task<IActionResult> generateSuggestion(string journaltext)
        {
            var result = await service.getSuggestion(journaltext);
            return Ok(result);
        }

    }
}
