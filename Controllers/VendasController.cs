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
        public async Task<ActionResult<IEnumerable<Venda>>> GetVenda
            (
            int tipo = 0,
            bool filtroData = false,
            DateTime filtroDataInicio = default,
            DateTime filtroDataFinal = default
            )
        {
            List<Venda> vendas;

            if(tipo >= 0)
            {
                vendas = await _context.Venda.Where(e => e.Tipo == tipo)
                    .Include(e => e.ItemVenda)
                    .ToListAsync();
            }
            else
            {
                vendas = await _context.Venda
                    .Include(e => e.ItemVenda)
                    .ToListAsync();
            }

            if(filtroData)
            {
                vendas = vendas.Where(e => e.HoraEntrada > filtroDataInicio & e.HoraEntrada < filtroDataFinal).ToList();
            }

            return vendas;
        }

        // GET: api/Vendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetVenda
            (
                int id,
                bool itemvendas = false,
                bool pagamentos = false
            )
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }



            if(itemvendas)
            {
                await _context.Entry(venda).Collection(e => e.ItemVenda)
                        .Query()
                        .Include(e => e.IditemNavigation)
                        .LoadAsync();
            }
            else
            {
                await _context.Entry(venda).Collection(e => e.ItemVenda)
                        .Query()
                        .LoadAsync();
            }

            if (pagamentos)
            {
                await _context.Entry(venda).Collection(e => e.Pagamento)
                    .Query()
                    .Include(e => e.IdmovimentoNavigation)
                    .ThenInclude(e => e.IdformaPagamentoNavigation)
                    .LoadAsync();
            }

            if(venda.Idcliente != null)
            {
                await _context.Entry(venda).Reference(e => e.IdclienteNavigation)
                    .LoadAsync();
            }

            return venda;
        }

        // GET: api/Vendas/Actions/Aberta
        [HttpGet("actions/aberta")]
        public async Task<ActionResult<int>> GetVendaAberta(int tipo = 0)
        {
            var venda = await _context.Venda.Where(e => e.Aberta == 1 & e.Tipo == tipo).FirstOrDefaultAsync();

            if (venda == null)
            {
                return 0;
            }

            return venda.Idvenda;
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
            venda.NumeroVenda = await Venda.GetLastNumero(_context);
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

            await _context.Entry(venda).Collection(e => e.ItemVenda).LoadAsync();

            pagamento.Idvenda = id;
            pagamento.IdmovimentoNavigation.IdformaPagamentoNavigation = null;
            pagamento.IdmovimentoNavigation.IdcaixaNavigation = null;

            if(venda.ValorPago == null)
            {
                venda.ValorPago = 0;
            }

            venda.ValorPago += pagamento.IdmovimentoNavigation.Valor;

            if (venda.ValorPago >= venda.ValorTotal)
            {
                venda.Paga = 1;
            }

            if (venda.ValorPago > venda.ValorTotal)
            {
                pagamento.IdmovimentoNavigation.Valor -= (venda.ValorPago ?? default) - venda.ValorTotal;
            }

            _context.Pagamento.Add(pagamento);
            _context.Entry(venda).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda, pagamentos = true, itemsvenda = true });
        }



        [HttpPost("{id}/itemvendas")]
        public async Task<ActionResult<Venda>> PostItemvenda(int id, ItemVenda itemVenda)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            await venda.AddItemVenda(itemVenda, _context);

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda, itemvendas = true });
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

            await venda.FecharVenda(_context);

            return NoContent();
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Idvenda == id);
        }

        // Get: api/Vendas/5/actions/recontaritems
        [HttpGet("{id}/actions/recontaritems")]
        public async Task<ActionResult<Venda>> RecontarItems(int id)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            await venda.RecontarIndices(_context);

            return NoContent();
        }
    }
}
