using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProfkomBackend.Data;
using ProfkomBackend.Models;

namespace ProfkomBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UnitController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ GET: api/unit - доступно всім
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Unit>>> GetAll()
        {
            return await _db.Unit
                .Where(u => u.isActive) // показуємо тільки активні
                .ToListAsync();
        }

        // ✅ GET: api/unit/5 - доступно всім
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> GetById(int id)
        {
            var unit = await _db.Unit.FindAsync(id);
            if (unit == null) return NotFound();
            return unit;
        }

        // 🔒 POST: api/unit - тільки адмін
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Unit>> Create(Unit unit)
        {
            _db.Unit.Add(unit);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = unit.Id }, unit);
        }

        // 🔒 PUT: api/unit/5 - тільки адмін
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, Unit unit)
        {
            if (id != unit.Id) return BadRequest();

            _db.Entry(unit).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 DELETE: api/unit/5 - тільки адмін
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _db.Unit.FindAsync(id);
            if (unit == null) return NotFound();

            _db.Unit.Remove(unit);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
