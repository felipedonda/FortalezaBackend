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
    public class FormaPagamentosController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public FormaPagamentosController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/FormaPagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormaPagamento>>> GetFormaPagamento()
        {
            return await _context.FormaPagamento.ToListAsync();
        }

        // GET: api/FormaPagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaPagamento>> GetFormaPagamento(int id)
        {
            var formaPagamento = await _context.FormaPagamento.FindAsync(id);

            if (formaPagamento == null)
            {
                return NotFound();
            }

            return formaPagamento;
        }

        // PUT: api/FormaPagamentos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormaPagamento(int id, FormaPagamento formaPagamento)
        {
            if (id != formaPagamento.IdformaPagamento)
            {
                return BadRequest();
            }

            _context.Entry(formaPagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormaPagamentoExists(id))
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

        // POST: api/FormaPagamentos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FormaPagamento>> PostFormaPagamento(FormaPagamento formaPagamento)
        {
            _context.FormaPagamento.Add(formaPagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFormaPagamento", new { id = formaPagamento.IdformaPagamento }, formaPagamento);
        }

        // DELETE: api/FormaPagamentos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormaPagamento>> DeleteFormaPagamento(int id)
        {
            var formaPagamento = await _context.FormaPagamento.FindAsync(id);
            if (formaPagamento == null)
            {
                return NotFound();
            }

            _context.FormaPagamento.Remove(formaPagamento);
            await _context.SaveChangesAsync();

            return formaPagamento;
        }

        private bool FormaPagamentoExists(int id)
        {
            return _context.FormaPagamento.Any(e => e.IdformaPagamento == id);
        }
    }
}
