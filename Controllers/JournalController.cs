using JournalApi.Data;
using JournalApi.DTO;
using JournalApi.Models;
using JournalApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JournalApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService service;
        public JournalController(IJournalService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult getJournal()
        {
          return Ok(service.GetJournalEntries(User.Identity.Name));
        }

        [HttpGet("{id}")]
        public IActionResult findById(int id)
        {
            
            return Ok(service.GetJournalEntryByID(id,User.Identity.Name));

         }
        [HttpPost("create")]
        
        public IActionResult createJournal(JournalEntryDTO dTO)
        {
            return Ok(service.CreateJournalEntry(dTO, User.Identity.Name));
        }


        [HttpPut("{id}")]
        public IActionResult updateEntries(JournalEntryDTO dto, int id)
        {
          return Ok(service.UpdateJournalEntry(id, User.Identity.Name,dto));
        }


        [HttpDelete("{id}")]
        public IActionResult deleteJournal(int id)
        {
          return Ok(service.DeleteJournalEntry(id, User.Identity.Name));
        }    


    }
}
