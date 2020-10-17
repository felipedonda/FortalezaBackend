using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortalezaServer.Models;

namespace FortalezaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenciasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public PreferenciasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Preferencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preferencias>>> GetPreferencias()
        {
            return await _context.Preferencias.ToListAsync();
        }

        // GET: api/Preferencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preferencias>> GetPreferencias(int id)
        {
            var preferencias = await _context.Preferencias.FindAsync(id);

            if (preferencias == null)
            {
                return NotFound();
            }

            return preferencias;
        }

        // PUT: api/Preferencias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreferencias(int id, Preferencias preferencias)
        {
            if (id != preferencias.Idpreferencias)
            {
                return BadRequest();
            }

            _context.Entry(preferencias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenciasExists(id))
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

        // POST: api/Preferencias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Preferencias>> PostPreferencias(Preferencias preferencias)
        {
            _context.Preferencias.Add(preferencias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreferencias", new { id = preferencias.Idpreferencias }, preferencias);
        }

        // DELETE: api/Preferencias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Preferencias>> DeletePreferencias(int id)
        {
            var preferencias = await _context.Preferencias.FindAsync(id);
            if (preferencias == null)
            {
                return NotFound();
            }

            _context.Preferencias.Remove(preferencias);
            await _context.SaveChangesAsync();

            return preferencias;
        }

        private bool PreferenciasExists(int id)
        {
            return _context.Preferencias.Any(e => e.Idpreferencias == id);
        }
    }
}
