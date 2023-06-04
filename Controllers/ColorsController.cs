using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.DAL;
using CarRental.Models;
using System.Net;
using CarRental.Entities.Dtos.Colors;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Colors
        [HttpGet("GetColors")]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            if (_context.Colors == null)
            {
                return NotFound();
            }
            return await _context.Colors.ToListAsync();
        }

        // GET: api/Colors/5
        [HttpGet("GetColor/{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            if (_context.Colors == null)
            {
                return NotFound();
            }
            Color? color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.OK, color);
        }


        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(CreateColorDto createColor)
        {
            if (_context.Colors == null)
            {
                return Problem("Entity set 'AppDbContext.Colors'  is null.");
            }

            Color color = new()
            {
                Name = createColor.Name
            };
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Created, color);
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, UpdateColorDto updateColor)
        {
            Color? oldColor = await _context.Colors.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (oldColor == null) return NotFound();

            oldColor.Name = updateColor.Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            if (_context.Colors == null)
            {
                return NotFound();
            }
            Color? color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
