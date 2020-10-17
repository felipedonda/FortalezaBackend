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
    public class TipoEntregadoresController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public TipoEntregadoresController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/TipoEntregadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEntregador>>> GetTipoEntregador()
        {
            return await _context.TipoEntregador.ToListAsync();
        }

        // GET: api/TipoEntregadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEntregador>> GetTipoEntregador(int id)
        {
            var tipoEntregador = await _context.TipoEntregador.FindAsync(id);

            if (tipoEntregador == null)
            {
                return NotFound();
            }

            return tipoEntregador;
        }

        // PUT: api/TipoEntregadores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoEntregador(int id, TipoEntregador tipoEntregador)
        {
            if (id != tipoEntregador.IdtipoEntregador)
            {
                return BadRequest();
            }

            _context.Entry(tipoEntregador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEntregadorExists(id))
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

        // POST: api/TipoEntregadores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TipoEntregador>> PostTipoEntregador(TipoEntregador tipoEntregador)
        {
            _context.TipoEntregador.Add(tipoEntregador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoEntregador", new { id = tipoEntregador.IdtipoEntregador }, tipoEntregador);
        }

        // DELETE: api/TipoEntregadores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoEntregador>> DeleteTipoEntregador(int id)
        {
            var tipoEntregador = await _context.TipoEntregador.FindAsync(id);
            if (tipoEntregador == null)
            {
                return NotFound();
            }

            _context.TipoEntregador.Remove(tipoEntregador);
            await _context.SaveChangesAsync();

            return tipoEntregador;
        }

        private bool TipoEntregadorExists(int id)
        {
            return _context.TipoEntregador.Any(e => e.IdtipoEntregador == id);
        }
    }
}
