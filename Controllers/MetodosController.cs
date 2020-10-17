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
    public class MetodosController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public MetodosController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Metodos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Metodo>>> GetMetodo()
        {
            return await _context.Metodo.ToListAsync();
        }

        // GET: api/Metodos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Metodo>> GetMetodo(int id)
        {
            var metodo = await _context.Metodo.FindAsync(id);

            if (metodo == null)
            {
                return NotFound();
            }

            return metodo;
        }

        // PUT: api/Metodos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetodo(int id, Metodo metodo)
        {
            if (id != metodo.Idmetodo)
            {
                return BadRequest();
            }

            _context.Entry(metodo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodoExists(id))
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

        // POST: api/Metodos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Metodo>> PostMetodo(Metodo metodo)
        {
            _context.Metodo.Add(metodo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMetodo", new { id = metodo.Idmetodo }, metodo);
        }

        // DELETE: api/Metodos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Metodo>> DeleteMetodo(int id)
        {
            var metodo = await _context.Metodo.FindAsync(id);
            if (metodo == null)
            {
                return NotFound();
            }

            _context.Metodo.Remove(metodo);
            await _context.SaveChangesAsync();

            return metodo;
        }

        private bool MetodoExists(int id)
        {
            return _context.Metodo.Any(e => e.Idmetodo == id);
        }
    }
}
