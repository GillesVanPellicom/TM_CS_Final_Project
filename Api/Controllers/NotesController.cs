using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly NotesDbContext _db;

    public NotesController(NotesDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> Get()
    {
        var notes = await _db.Notes.OrderByDescending(n => n.Date).ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Note>> Get(int id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note == null) return NotFound();
        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> Post([FromBody] Note note)
    {
        note.Date = note.Date == default ? DateTime.UtcNow : note.Date;
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Note note)
    {
        if (id != note.Id) return BadRequest();
        var existingNote = await _db.Notes.FindAsync(id);
        if (existingNote == null) return NotFound();

        existingNote.Title = note.Title;
        existingNote.Content = note.Content;
        existingNote.Date = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note == null) return NotFound();
        _db.Notes.Remove(note);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Note>>> Search([FromQuery] string query)
    {
        query = query ?? string.Empty;
        var lowered = query.ToLower();
        var notes = await _db.Notes
            .Where(n => n.Title.ToLower().Contains(lowered) || n.Content.ToLower().Contains(lowered))
            .OrderByDescending(n => n.Date)
            .ToListAsync();
        return Ok(notes);
    }
}