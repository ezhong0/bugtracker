using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BugTracker.Models;
using Microsoft.AspNetCore.Cors;

namespace BugTracker.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly Database _context;

        public TicketsController(Database context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            var tickets = await _context.Tickets.ToListAsync();

            var result = tickets.Select(t => new Ticket
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Description = t.Description,
                PriorityId = t.PriorityId,
                StatusId = t.StatusId,
                TypeId = t.TypeId,
                UserCreatedId = t.UserCreatedId,
                UserAssignedId = t.UserAssignedId,
                ProjectId = t.ProjectId,
                UserCreated = _context.Users.Find(t.UserCreatedId),
                UserAssigned = _context.Users.Find(t.UserAssignedId),
                Project = _context.Projects.Find(t.ProjectId)
            });


            return result.ToList();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutTicket(int id, int userId, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            var entry = _context.Entry(ticket);
            foreach (var item in entry.CurrentValues.Properties)
            {
                var propEntry = entry.Property(item.Name);
                if (propEntry.IsModified)
                {
                    _context.Historys.Add(new History
                    {
                        DateModified = DateTime.Now,
                        TicketId = ticket.TicketId,
                        Attribute = item.Name,
                        OldValue = propEntry.OriginalValue.ToString(),
                        NewValue = propEntry.CurrentValue.ToString(),
                        UserId = userId,
                        Ticket = ticket
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{userId}")]
        public async Task<ActionResult<Ticket>> PostTicket(int userId, Ticket ticket)
        {

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            ticket.UserCreatedId = userId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }

        // DELETE: api/Tickets/5s
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return ticket;
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}