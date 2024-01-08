using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers_Api
{
    [Route("api/v1/knjiga")]
    [ApiController]
    public class KnjigaApiController : ControllerBase
    {
        private readonly SchoolContext _context;

        public KnjigaApiController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/v1/knjiga
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Knjiga>>> GetKnjige()
        {
            return await _context.Knjige.ToListAsync();
        }

        // GET: /api/v1/knjiga/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Knjiga>> GetKnjiga(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);

            if (knjiga == null)
            {
                return NotFound();
            }

            return knjiga;
        }

        // PUT: api/v1/knjiga/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKnjiga(int id, Knjiga knjiga)
        {
            if (id != knjiga.KnjigaId)
            {
                return BadRequest();
            }

            _context.Entry(knjiga).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KnjigaExists(id))
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

        // POST: api/v1/knjiga
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Knjiga>> PostKnjiga(Knjiga knjiga)
        {
            _context.Knjige.Add(knjiga);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKnjiga", new { id = knjiga.KnjigaId }, knjiga);
        }

        // DELETE: api/v1/knjiga
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnjiga(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga == null)
            {
                return NotFound();
            }

            _context.Knjige.Remove(knjiga);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.KnjigaId == id);
        }
    }
}
