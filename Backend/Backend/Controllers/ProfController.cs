using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProfkomBackend.Data;
using ProfkomBackend.Models;

namespace ProfkomBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProfController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ GET: api/prof - доступно всім
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Prof>>> GetAll()
        {
            return await _db.Prof
                .Where(p => p.isActive) // можна показувати тільки активні
                .ToListAsync();
        }

        // ✅ GET: api/prof/5 - доступно всім
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Prof>> GetById(int id)
        {
            var prof = await _db.Prof.FindAsync(id);
            if (prof == null) return NotFound();
            return prof;
        }

        // 🔒 POST: api/prof - тільки адмін
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Prof>> Create(Prof prof)
        {
            _db.Prof.Add(prof);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = prof.Id }, prof);
        }

        // 🔒 PUT: api/prof/5 - тільки адмін
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, Prof prof)
        {
            if (id != prof.Id) return BadRequest();

            _db.Entry(prof).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 DELETE: api/prof/5 - тільки адмін
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var prof = await _db.Prof.FindAsync(id);
            if (prof == null) return NotFound();

            _db.Prof.Remove(prof);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
