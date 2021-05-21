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
    public class PdvsController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public PdvsController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Pdvs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pdv>>> GetPdv()
        {
            return await _context.Pdv.ToListAsync();
        }

        // GET: api/Pdvs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pdv>> GetPdv(int id)
        {
            var pdv = await _context.Pdv.FindAsync(id);

            if (pdv == null)
            {
                return NotFound();
            }

            return pdv;
        }

        // PUT: api/Pdvs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPdv(int id, Pdv pdv)
        {
            if (id != pdv.Idpdv)
            {
                return BadRequest();
            }

            _context.Entry(pdv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PdvExists(id))
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

        // POST: api/Pdvs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pdv>> PostPdv(Pdv pdv)
        {
            _context.Pdv.Add(pdv);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPdv", new { id = pdv.Idpdv }, pdv);
        }

        // DELETE: api/Pdvs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pdv>> DeletePdv(int id)
        {
            var pdv = await _context.Pdv.FindAsync(id);
            if (pdv == null)
            {
                return NotFound();
            }

            _context.Pdv.Remove(pdv);
            await _context.SaveChangesAsync();

            return pdv;
        }

        private bool PdvExists(int id)
        {
            return _context.Pdv.Any(e => e.Idpdv == id);
        }
    }
}
