using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Database;
using NotesAPI.Models;

namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class NotesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public NotesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotesModel>>> GetNotes()
        {
            Console.WriteLine(User.Identity.IsAuthenticated);
            return await _context.Notes.ToListAsync();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotesModel>> GetNotesModel(int id)
        {
            var notesModel = await _context.Notes.FindAsync(id);

            if (notesModel == null)
            {
                return NotFound();
            }

            return notesModel;
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotesModel(int id, NotesModel notesModel)
        {
            if (id != notesModel.NotesID)
            {
                return BadRequest();
            }

            _context.Entry(notesModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesModelExists(id))
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

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotesModel>> PostNotesModel(NotesModel notesModel)
        {
            _context.Notes.Add(notesModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotesModel", new { id = notesModel.NotesID }, notesModel);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotesModel(int id)
        {
            var notesModel = await _context.Notes.FindAsync(id);
            if (notesModel == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(notesModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotesModelExists(int id)
        {
            return _context.Notes.Any(e => e.NotesID == id);
        }
    }
}
