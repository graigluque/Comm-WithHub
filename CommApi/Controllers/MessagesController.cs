using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommApi.Models;
using CommApi.Data;
using CommApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CommApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDbContext _context;
        private IHubContext<ChatHub> _hubContext;

        public MessagesController(IDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _context.EnsureCreated();
            _hubContext = hubContext;
        }

        // GET: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            if (_context.Messages == null)
                return NotFound();

            return await _context.Messages.ToListAsync();
        }

        // GET: api/Messages/1001
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("{groupId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(string groupId)
        {
            if (_context.Messages == null)
                return NotFound();
            var messages = await _context.Messages.Where(e =>
                                        e.GroupId == groupId)
                                    .OrderByDescending(e => e.CreatedAt)
                                    .ToListAsync();

            if (messages == null)
                return NotFound();

            return messages;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(string id, Message message)
        {
            if (id != message.Id)
                return BadRequest();

            _context.SetModifyState(message);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'MessageContext.Messages'  is null.");
            }
            _context.SetMessageId(message);
            message.CreatedAt = DateTime.Now;

            _context.Messages.Add(message);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (message.Id == null || MessageExists(message.Id))
                    return Conflict();
                else
                    throw;
            };

            // Notify all connected Clients with SignalRService          
            Console.WriteLine("[Custom msg]: NotifyToGroup sending ...");
            Console.WriteLine(message);
            await _hubContext.Clients.All.SendAsync(message.GroupId, message);

            Console.WriteLine("[Custom msg]: BroadcastMessage sending ...");
            await _hubContext.Clients.All.SendAsync("public", "New message for " + message.GroupId);

            // return new message with new Message Id
            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);

        }

        // DELETE: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            if (_context.Messages == null)
                return NotFound();
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return NotFound();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(string? id)
        {
            return (_context.Messages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
