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
    public class EntregadoresController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public EntregadoresController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Entregadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entregador>>> GetEntregador()
        {
            return await _context.Entregador.ToListAsync();
        }

        // GET: api/Entregadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entregador>> GetEntregador(int id)
        {
            var entregador = await _context.Entregador.FindAsync(id);

            if (entregador == null)
            {
                return NotFound();
            }

            return entregador;
        }

        // PUT: api/Entregadores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntregador(int id, Entregador entregador)
        {
            if (id != entregador.Identregador)
            {
                return BadRequest();
            }

            _context.Entry(entregador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntregadorExists(id))
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

        // POST: api/Entregadores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Entregador>> PostEntregador(Entregador entregador)
        {
            _context.Entregador.Add(entregador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntregador", new { id = entregador.Identregador }, entregador);
        }

        // DELETE: api/Entregadores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Entregador>> DeleteEntregador(int id)
        {
            var entregador = await _context.Entregador.FindAsync(id);
            if (entregador == null)
            {
                return NotFound();
            }

            _context.Entregador.Remove(entregador);
            await _context.SaveChangesAsync();

            return entregador;
        }

        private bool EntregadorExists(int id)
        {
            return _context.Entregador.Any(e => e.Identregador == id);
        }
    }
}
