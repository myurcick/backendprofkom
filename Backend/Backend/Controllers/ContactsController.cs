using Microsoft.AspNetCore.Mvc;
using ProfkomBackend.Data;
using ProfkomBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ProfkomBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ContactsController(AppDbContext db) => _db = db;

        [HttpPost("send")]
        public async Task<IActionResult> Send(ContactMessage msg)
        {
            _db.ContactMessages.Add(msg);
            await _db.SaveChangesAsync();
            // TODO: send email/notification if required
            return Ok(new { success = true });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.ContactMessages.OrderByDescending(m => m.SentAt).ToListAsync());
    }
}
