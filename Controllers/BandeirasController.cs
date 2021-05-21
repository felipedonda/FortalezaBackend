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
            return await _context.Bandeira.OrderBy(e => e.Ordem).ToListAsync();
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


        // GET: api/Bandeiras/actions/ordem
        [HttpGet("actions/ordem")]
        public async Task<ActionResult<int>> GetOrdem()
        {
            Bandeira lastObject = await _context.Bandeira.OrderByDescending(e => e.Ordem).FirstOrDefaultAsync();
            return lastObject.Ordem + 1;
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

            Bandeira bandeiraMesmaOrdem = _context.Bandeira.Where(e => e.Ordem == bandeira.Ordem && e.Idbandeira != bandeira.Idbandeira).FirstOrDefault();

            if (bandeiraMesmaOrdem != null)
            {
                Bandeira bandeiraAntiga = await _context.Bandeira.FindAsync(id);
                bandeiraMesmaOrdem.Ordem = bandeiraAntiga.Ordem;
                _context.Entry(bandeiraAntiga).State = EntityState.Detached;
                _context.Entry(bandeiraMesmaOrdem).State = EntityState.Modified;
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
            Bandeira bandeiraMesmaOrdem = _context.Bandeira.Where(e => e.Ordem == bandeira.Ordem).FirstOrDefault();

            if (bandeiraMesmaOrdem != null)
            {
                bandeiraMesmaOrdem.Ordem = (await GetOrdem()).Value;
                _context.Entry(bandeiraMesmaOrdem).State = EntityState.Modified;
            }

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
