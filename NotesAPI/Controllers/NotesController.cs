using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Database;
using NotesAPI.Models;


namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<UserModel> _userManager;

        public NotesController(DatabaseContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    
        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotesModel>>> GetNotes()
        {
            var idToken = Request.HttpContext?.User.FindFirst("sub")?.Value ?? string.Empty;

            var notes = await _context.Notes.Where(n => n.ByUser.Id == idToken).ToListAsync();
            return Ok(notes);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotesModel>> GetNotesModel([FromRoute] int id)
        {
            var idToken = Request.HttpContext?.User.FindFirst("sub")?.Value ?? string.Empty;

            var notesModel = await _context.Notes.Where(nt => nt.NotesID == id  && nt.ByUser.Id == idToken).ToListAsync();

            
            if (notesModel == null)
            {
                return NotFound();
            }

            return Ok(notesModel);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchNotesModel([FromRoute] int id,[FromBody] NotesModel notesModel) {

            if ( (notesModel?.NotesID) == null)
                return BadRequest();
            
            NotesModel NoteChoosen = await _context.Notes.FirstOrDefaultAsync(x => x.NotesID == id);

            try
            {
                if(NoteChoosen != null)
                {
                    if (notesModel.Title != null) NoteChoosen.Title = notesModel.Title;
                    if(notesModel.Description != null) NoteChoosen.Description = notesModel.Description;

                    await _context.SaveChangesAsync();
                }
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

            return Ok("Updated Note :)");
        }


        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NotesModel>> PostNotesModel(NotesModel notesModel)
        {

            // Pegar Usuario pelo Token Enviado
            var idToken = Request.HttpContext?.User.FindFirst("sub")?.Value ?? string.Empty;
            var usrDt = await _userManager.FindByIdAsync(idToken);
                
            if(usrDt == null) return NotFound("User not exist to create a Note. Contact support.");

            notesModel.ByUser = usrDt;
            _context.Notes.Add(notesModel);
            await _context.SaveChangesAsync();

            return Ok("Your Note was sucessfully registered in Database ;) ");
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

            return Ok("Your note was sucessfully deleted from Database.");
        }

        private bool NotesModelExists(int id)
        {
            return _context.Notes.Any(e => e.NotesID == id);
        }
    }
}
