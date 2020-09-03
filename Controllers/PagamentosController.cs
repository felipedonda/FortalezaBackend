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
    public class PagamentosController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public PagamentosController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Pagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamento()
        {
            return await _context.Pagamento.ToListAsync();
        }

        // GET: api/Pagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamento(int id)
        {
            var pagamento = await _context.Pagamento.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound();
            }

            return pagamento;
        }

        // PUT: api/Pagamentos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamento(int id, Pagamento pagamento)
        {
            if (id != pagamento.VendaIdvenda)
            {
                return BadRequest();
            }

            _context.Entry(pagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagamentoExists(id))
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

        // POST: api/Pagamentos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(Pagamento pagamento)
        {
            _context.Pagamento.Add(pagamento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PagamentoExists(pagamento.VendaIdvenda))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPagamento", new { id = pagamento.VendaIdvenda }, pagamento);
        }

        // DELETE: api/Pagamentos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pagamento>> DeletePagamento(int id)
        {
            var pagamento = await _context.Pagamento.FindAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }

            _context.Pagamento.Remove(pagamento);
            await _context.SaveChangesAsync();

            return pagamento;
        }

        private bool PagamentoExists(int id)
        {
            return _context.Pagamento.Any(e => e.VendaIdvenda == id);
        }
    }
}
