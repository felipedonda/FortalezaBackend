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
    public class PacotesController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public PacotesController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Pacotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pacote>>> GetPacote()
        {
            return await _context.Pacote.ToListAsync();
        }

        // GET: api/Pacotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pacote>> GetPacote(int id)
        {
            var pacote = await _context.Pacote.FindAsync(id);

            if (pacote == null)
            {
                return NotFound();
            }

            return pacote;
        }

        // PUT: api/Pacotes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPacote(int id, Pacote pacote)
        {
            if (id != pacote.Iditem)
            {
                return BadRequest();
            }

            _context.Entry(pacote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacoteExists(id))
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

        // POST: api/Pacotes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pacote>> PostPacote(Pacote pacote)
        {
            _context.Pacote.Add(pacote);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PacoteExists(pacote.Iditem))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPacote", new { id = pacote.Iditem }, pacote);
        }

        // DELETE: api/Pacotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pacote>> DeletePacote(int id)
        {
            var pacote = await _context.Pacote.FindAsync(id);
            if (pacote == null)
            {
                return NotFound();
            }

            _context.Pacote.Remove(pacote);
            await _context.SaveChangesAsync();

            return pacote;
        }

        private bool PacoteExists(int id)
        {
            return _context.Pacote.Any(e => e.Iditem == id);
        }
    }
}
