using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommApi.Models;
using CommApi.Data;

namespace CommApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IDbContext _context;


        public GroupsController(IDbContext context)
        {
            _context = context;
            _context.EnsureCreated();
        }

        // GET: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }

            return await _context.Groups.ToListAsync();
        }

        // GET: api/Groups/1001
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(string id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'CommContext.Groups'  is null.");
            }
            group.CreatedAt = DateTime.Now;

            _context.Groups.Add(group);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (group.Id == null || GroupExists(group.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        // DELETE: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(string? id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
