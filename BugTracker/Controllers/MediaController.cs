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
    public class MediasController : ControllerBase
    {
        private readonly Database _context;

        public MediasController(Database context)
        {
            _context = context;
        }

        // GET: api/Medias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
        {
            var medias = await _context.Medias.ToListAsync();

            var result = medias.Select(t => new Media
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Data = t.Data,
                Ticket = _context.Tickets.Find(t.TicketId)
            });


            return result.ToList();
        }

        // GET: api/Medias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Media>> GetMedia(int id)
        {
            var media = await _context.Medias.FindAsync(id);

            if (media == null)
            {
                return NotFound();
            }

            return media;
        }

        // PUT: api/Medias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}/{userId}")]
        public async Task<IActionResult> PutMedia(int id, int userId, Media media)
        {
            if (id != media.MediaId)
            {
                return BadRequest();
            }

            _context.Entry(media).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(id))
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

        // POST: api/Medias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{userId}")]
        public async Task<ActionResult<Media>> PostMedia(int userId, Media media)
        {

            _context.Medias.Add(media);

            await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


            return CreatedAtAction("GetMedia", new { id = media.MediaId }, media);
        }

        // DELETE: api/Medias/5s
        [HttpDelete("{id}")]
        public async Task<ActionResult<Media>> DeleteMedia(int id)
        {
            var media = await _context.Medias.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            _context.Medias.Remove(media);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return media;
        }

        private bool MediaExists(int id)
        {
            return _context.Medias.Any(e => e.MediaId == id);
        }
    }
}