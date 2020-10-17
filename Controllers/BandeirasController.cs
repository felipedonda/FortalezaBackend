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
    public class BandeirasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public BandeirasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Bandeiras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bandeira>>> GetBandeira()
        {
            return await _context.Bandeira.ToListAsync();
        }

        // GET: api/Bandeiras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bandeira>> GetBandeira(int id)
        {
            var bandeira = await _context.Bandeira.FindAsync(id);

            if (bandeira == null)
            {
                return NotFound();
            }

            return bandeira;
        }

        // PUT: api/Bandeiras/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBandeira(int id, Bandeira bandeira)
        {
            if (id != bandeira.Idbandeira)
            {
                return BadRequest();
            }

            _context.Entry(bandeira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BandeiraExists(id))
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

        // POST: api/Bandeiras
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Bandeira>> PostBandeira(Bandeira bandeira)
        {
            _context.Bandeira.Add(bandeira);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBandeira", new { id = bandeira.Idbandeira }, bandeira);
        }

        // DELETE: api/Bandeiras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bandeira>> DeleteBandeira(int id)
        {
            var bandeira = await _context.Bandeira.FindAsync(id);
            if (bandeira == null)
            {
                return NotFound();
            }

            _context.Bandeira.Remove(bandeira);
            await _context.SaveChangesAsync();

            return bandeira;
        }

        private bool BandeiraExists(int id)
        {
            return _context.Bandeira.Any(e => e.Idbandeira == id);
        }
    }
}
