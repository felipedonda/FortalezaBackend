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
            return await _context.FormaPagamento.OrderBy(e => e.Ordem).ToListAsync();
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

        // GET: api/FormaPagamentos/actions/ordem
        [HttpGet("actions/ordem")]
        public async Task<ActionResult<int>> GetOrdem()
        {
            FormaPagamento lastObject = await _context.FormaPagamento.OrderByDescending(e => e.Ordem).FirstOrDefaultAsync();
            return lastObject.Ordem + 1;
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

            FormaPagamento fpMesmaOrdem = _context.FormaPagamento.Where(e => e.Ordem == formaPagamento.Ordem && e.IdformaPagamento != formaPagamento.IdformaPagamento).FirstOrDefault();

            if(fpMesmaOrdem != null)
            {
                FormaPagamento fpAntiga = await _context.FormaPagamento.FindAsync(id);
                fpMesmaOrdem.Ordem = fpAntiga.Ordem;
                _context.Entry(fpAntiga).State = EntityState.Detached;
                _context.Entry(fpMesmaOrdem).State = EntityState.Modified;
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
            FormaPagamento fpMesmaOrdem = _context.FormaPagamento.Where(e => e.Ordem == formaPagamento.Ordem).FirstOrDefault();

            if (fpMesmaOrdem != null)
            {
                fpMesmaOrdem.Ordem = (await GetOrdem()).Value;
                _context.Entry(fpMesmaOrdem).State = EntityState.Modified;
            }

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
