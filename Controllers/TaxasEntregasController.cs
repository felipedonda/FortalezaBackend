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
    public class TaxasEntregasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public TaxasEntregasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/TaxasEntregas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxasEntrega>>> GetTaxasEntrega()
        {
            return await _context.TaxasEntrega.ToListAsync();
        }

        // GET: api/TaxasEntregas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaxasEntrega>> GetTaxasEntrega(int id)
        {
            var taxasEntrega = await _context.TaxasEntrega.FindAsync(id);

            if (taxasEntrega == null)
            {
                return NotFound();
            }

            return taxasEntrega;
        }

        // PUT: api/TaxasEntregas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxasEntrega(int id, TaxasEntrega taxasEntrega)
        {
            if (id != taxasEntrega.IdtaxasEntrega)
            {
                return BadRequest();
            }

            _context.Entry(taxasEntrega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxasEntregaExists(id))
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

        // POST: api/TaxasEntregas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TaxasEntrega>> PostTaxasEntrega(TaxasEntrega taxasEntrega)
        {
            _context.TaxasEntrega.Add(taxasEntrega);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaxasEntrega", new { id = taxasEntrega.IdtaxasEntrega }, taxasEntrega);
        }

        // DELETE: api/TaxasEntregas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaxasEntrega>> DeleteTaxasEntrega(int id)
        {
            var taxasEntrega = await _context.TaxasEntrega.FindAsync(id);
            if (taxasEntrega == null)
            {
                return NotFound();
            }

            _context.TaxasEntrega.Remove(taxasEntrega);
            await _context.SaveChangesAsync();

            return taxasEntrega;
        }

        private bool TaxasEntregaExists(int id)
        {
            return _context.TaxasEntrega.Any(e => e.IdtaxasEntrega == id);
        }
    }
}
