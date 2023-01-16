﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        private string getUserEmail() { return User.FindFirstValue(ClaimTypes.Email) ?? "";  }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotesModel>>> GetNotes()
        {
            string userEmail = getUserEmail();
            if (userEmail == "")
                return BadRequest("Invalid credentials, try to login again.");

            var notes = await _context.Notes.Where(u => u.ByUser.Email == userEmail).ToListAsync();
            return Ok(notes);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotesModel>> GetNotesModel([FromRoute] int id)
        {
            string userEmail = getUserEmail();
            if (userEmail == "")
                return BadRequest("Invalid credentials, try to login again.");

            var notesModel = await _context.Notes.Where(u => u.ByUser.Email == userEmail && u.NotesID == id).ToListAsync();

            if (notesModel == null)
            {
                return NotFound();
            }

            return Ok(notesModel);
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

            string userEmail = getUserEmail();
            if (userEmail == "")
                return BadRequest("Invalid credentials, try to login again.");

            notesModel.ByUser =  _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();
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

            return NoContent();
        }

        private bool NotesModelExists(int id)
        {
            return _context.Notes.Any(e => e.NotesID == id);
        }
    }
}