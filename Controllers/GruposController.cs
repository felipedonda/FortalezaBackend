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
    public class GruposController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public GruposController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Grupos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGrupo()
        {
            return await _context.Grupo.ToListAsync();
        }

        // GET: api/Grupos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(int id, bool items = false)
        {
            var grupo = await _context.Grupo.FindAsync(id);

            if (grupo == null)
            {
                return NotFound();
            }

            if(items)
            {
                await _context.Entry(grupo)
                    .Collection(e => e.ItemHasGrupo)
                    .Query()
                    .Include(e => e.ItemNavigation)
                    .LoadAsync();
            }

            return grupo;
        }

        // PUT: api/Grupos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
        {
            if (id != grupo.Idgrupo)
            {
                return BadRequest();
            }

            _context.Entry(grupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
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

        // POST: api/Grupos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
            _context.Grupo.Add(grupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrupo", new { id = grupo.Idgrupo }, grupo);
        }

        // DELETE: api/Grupos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Grupo>> DeleteGrupo(int id)
        {
            var grupo = await _context.Grupo.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }

            _context.Grupo.Remove(grupo);
            await _context.SaveChangesAsync();

            return grupo;
        }

        private bool GrupoExists(int id)
        {
            return _context.Grupo.Any(e => e.Idgrupo == id);
        }
    }
}
