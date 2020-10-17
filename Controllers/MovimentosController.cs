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
    public class MovimentosController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public MovimentosController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Movimentoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimento>>> GetMovimento()
        {
            return await _context.Movimento.ToListAsync();
        }

        // GET: api/Movimentoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimento>> GetMovimento(int id)
        {
            var movimento = await _context.Movimento.FindAsync(id);

            if (movimento == null)
            {
                return NotFound();
            }

            return movimento;
        }

        // PUT: api/Movimentoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimento(int id, Movimento movimento)
        {
            if (id != movimento.Idmovimento)
            {
                return BadRequest();
            }

            var oldMovimento = await _context.Movimento.FindAsync(id);

            if (oldMovimento == null)
            {
                return NotFound();
            }

            await _context.Entry(oldMovimento)
                .Collection(e => e.Pagamento)
                .Query()
                .Include(e => e.IdvendaNavigation)
                .LoadAsync();

            if (oldMovimento.Pagamento != null)
            {
                var pagamento = oldMovimento.Pagamento.FirstOrDefault();
                pagamento.IdvendaNavigation.ValorPago -= oldMovimento.Valor;
                pagamento.IdvendaNavigation.ValorPago += movimento.Valor;
                _context.Entry(pagamento.IdvendaNavigation).State = EntityState.Modified;
            }

            _context.Entry(oldMovimento).State = EntityState.Detached;

            _context.Entry(movimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentoExists(id))
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

        // POST: api/Movimentoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Movimento>> PostMovimento(Movimento movimento)
        {
            _context.Movimento.Add(movimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimento", new { id = movimento.Idmovimento }, movimento);
        }

        // DELETE: api/Movimentoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movimento>> DeleteMovimento(int id)
        {
            var movimento = await _context.Movimento.FindAsync(id);
            if (movimento == null)
            {
                return NotFound();
            }

            await _context.Entry(movimento)
                .Collection(e => e.Pagamento)
                .Query()
                .Include(e => e.IdvendaNavigation)
                .LoadAsync();

            if (movimento.Pagamento != null)
            {
                var pagamento = movimento.Pagamento.FirstOrDefault();
                pagamento.IdvendaNavigation.ValorPago -= movimento.Valor;
                _context.Entry(pagamento.IdvendaNavigation).State = EntityState.Modified;
            }

            _context.Movimento.Remove(movimento);
            await _context.SaveChangesAsync();


            return movimento;
        }

        private bool MovimentoExists(int id)
        {
            return _context.Movimento.Any(e => e.Idmovimento == id);
        }
    }
}
