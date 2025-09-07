using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProfkomBackend.Data;
using ProfkomBackend.Models;

namespace ProfkomBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TeamController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ GET: api/team - доступно всім
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Team>>> GetAll()
        {
            return await _db.Team
                .Where(t => t.isActive) // наприклад, тільки активних
                .ToListAsync();
        }

        // ✅ GET: api/team/5 - доступно всім
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Team>> GetById(int id)
        {
            var member = await _db.Team.FindAsync(id);
            if (member == null) return NotFound();
            return member;
        }

        // 🔒 POST: api/team - тільки адмін
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Team>> Create(Team member)
        {
            _db.Team.Add(member);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }

        // 🔒 PUT: api/team/5 - тільки адмін
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, Team member)
        {
            if (id != member.Id) return BadRequest();

            _db.Entry(member).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 DELETE: api/team/5 - тільки адмін
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _db.Team.FindAsync(id);
            if (member == null) return NotFound();

            _db.Team.Remove(member);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
