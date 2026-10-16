using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayDB1.Data;
using RaceDayDB1.Models;


namespace RacedDayDB1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDBContext _context;
        public EventsController(RaceDayDBContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null) return NotFound();
            return e;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event ev)
        {
            if (HttpContext.Session.GetInt32("UserId") == null) return Unauthorized();
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return Forbid();
            ev.OrganiserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = ev.EventId }, ev);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event ev)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return Forbid();
            if (id != ev.EventId) return BadRequest();
            _context.Entry(ev).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return Forbid();
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}