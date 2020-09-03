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
    public class VendasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public VendasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Vendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> GetVenda()
        {
            return await _context.Venda.ToListAsync();
        }

        // GET: api/Vendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetVenda(int id, bool itemvendas = false, bool pagamentos = false)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            if (itemvendas)
            {
                await _context.Entry(venda).Collection(e => e.ItemVenda)
                    .Query()
                    .Include(e => e.ItemNavigation)
                    .LoadAsync();
            }

            if (pagamentos)
            {
                await _context.Entry(venda).Collection(e => e.Pagamento)
                    .Query()
                    .Include(e => e.Movimento)
                    .ThenInclude(e => e.FormaPagamentoNavigation)
                    .LoadAsync();
            }

            return venda;
        }

        // PUT: api/Vendas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenda(int id, Venda venda)
        {
            if (id != venda.Idvenda)
            {
                return BadRequest();
            }

            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
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

        // POST: api/Vendas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Venda>> PostVenda(Venda venda)
        {
            _context.Venda.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda }, venda);
        }

        [HttpPost("{id}/pagamentos")]
        public async Task<ActionResult<Venda>> PostPagamento(int id, Pagamento pagamento)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            pagamento.VendaIdvenda = id;
            pagamento.Movimento.FormaPagamentoNavigation = null;
            pagamento.Movimento.CaixaIdcaixaNavigation = null;

            _context.Pagamento.Add(pagamento);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda, pagamentos = true }, venda);
        }



        [HttpPost("{id}/itemvendas")]
        public async Task<ActionResult<Venda>> PostItemvenda(int id, ItemVenda itemVenda)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            itemVenda.VendaIdvenda = id;

            venda.ItemVenda.Add(itemVenda);
           
            await _context.SaveChangesAsync();

            venda.ValorTotal += itemVenda.Quantidade * itemVenda.Valor;
            _context.Entry(venda).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda, itemvendas = true }, venda);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Venda>> DeleteVenda(int id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            _context.Venda.Remove(venda);
            await _context.SaveChangesAsync();

            return venda;
        }

        // Get: api/Vendas/5/actions/fechar
        [HttpGet("{id}/actions/fechar")]
        public async Task<ActionResult<Venda>> FecharVenda(int id)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            await _context.Entry(venda).Collection(e => e.ItemVenda).LoadAsync();

            foreach(var item in venda.ItemVenda.ToList())
            {
                await item.MovimentarEstoqueVenda(_context);
            }

            venda.Aberta = 0;

            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(id))
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

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Idvenda == id);
        }
    }
}
