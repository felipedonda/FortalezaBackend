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
    public class CaixasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public CaixasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Caixas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caixa>>> GetCaixa()
        {
            return await _context.Caixa.ToListAsync();
        }

        [HttpGet("actions/aberto")]
        public async Task<ActionResult<Caixa>> GetCaixaAberto(bool movimentos = false)
        {
            var caixa = await _context.Caixa.Where(e => e.Aberto == 1).FirstOrDefaultAsync();

            if (caixa == null)
            {
                return NoContent();
            }

            if (movimentos)
            {
                await _context.Entry(caixa)
                    .Collection(e => e.Movimento)
                    .Query()
                    .Include(e => e.FormaPagamentoNavigation)
                    .LoadAsync();
            }

            return caixa;
        }

        // GET: api/Caixas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Caixa>> GetCaixa(int id, bool movimentos = false)
        {
            var caixa = await _context.Caixa.FindAsync(id);

            if (caixa == null)
            {
                return NotFound();
            }

            if (movimentos)
            {
                await _context.Entry(caixa)
                    .Collection(e => e.Movimento)
                    .LoadAsync();
            }

            return caixa;
        }

        // PUT: api/Caixas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaixa(int id, Caixa caixa)
        {
            if (id != caixa.Idcaixa)
            {
                return BadRequest();
            }

            _context.Entry(caixa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaixaExists(id))
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

        // POST: api/Caixas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Caixa>> PostCaixa(Caixa caixa)
        {
            _context.Caixa.Add(caixa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaixa", new { id = caixa.Idcaixa }, caixa);
        }

        // DELETE: api/Caixas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Caixa>> DeleteCaixa(int id)
        {
            var caixa = await _context.Caixa.FindAsync(id);
            if (caixa == null)
            {
                return NotFound();
            }

            _context.Caixa.Remove(caixa);
            await _context.SaveChangesAsync();

            return caixa;
        }

        private bool CaixaExists(int id)
        {
            return _context.Caixa.Any(e => e.Idcaixa == id);
        }
    }
}
