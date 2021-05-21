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
    public class NomeCaixasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public NomeCaixasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/NomeCaixas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NomeCaixa>>> GetNomeCaixa()
        {
            return await _context.NomeCaixa.ToListAsync();
        }

        // GET: api/NomeCaixas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NomeCaixa>> GetNomeCaixa(int id)
        {
            var nomeCaixa = await _context.NomeCaixa.FindAsync(id);

            if (nomeCaixa == null)
            {
                return NotFound();
            }

            return nomeCaixa;
        }

        // PUT: api/NomeCaixas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNomeCaixa(int id, NomeCaixa nomeCaixa)
        {
            if (id != nomeCaixa.IdnomeCaixa)
            {
                return BadRequest();
            }

            _context.Entry(nomeCaixa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NomeCaixaExists(id))
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

        // POST: api/NomeCaixas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NomeCaixa>> PostNomeCaixa(NomeCaixa nomeCaixa)
        {
            _context.NomeCaixa.Add(nomeCaixa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNomeCaixa", new { id = nomeCaixa.IdnomeCaixa }, nomeCaixa);
        }

        // DELETE: api/NomeCaixas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NomeCaixa>> DeleteNomeCaixa(int id)
        {
            var nomeCaixa = await _context.NomeCaixa.FindAsync(id);
            if (nomeCaixa == null)
            {
                return NotFound();
            }

            _context.NomeCaixa.Remove(nomeCaixa);
            await _context.SaveChangesAsync();

            return nomeCaixa;
        }

        private bool NomeCaixaExists(int id)
        {
            return _context.NomeCaixa.Any(e => e.IdnomeCaixa == id);
        }
    }
}
