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
    public class PedidosController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public PedidosController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedido(bool delivery = false)
        {
            List<Pedido> pedidos;

            if(delivery)
            {
                pedidos = await _context.Pedido
                .Where(e => e.Delivery == 1)
                .Include(e => e.IdvendaNavigation)
                .ThenInclude(e => e.IdresponsavelNavigation)
                .Include(e => e.IdentregadorNavigation)
                .ToListAsync();
            }
            else
            {
                pedidos = await _context.Pedido
                .Where(e => e.Delivery == 0)
                .Include(e => e.IdvendaNavigation)
                .ThenInclude(e => e.IdresponsavelNavigation)
                .Include(e => e.IdentregadorNavigation)
                .ToListAsync();
            }


            pedidos.ForEach(async pedido =>
            {
                if(pedido.IdentregadorNavigation != null)
                {
                    pedido.IdentregadorNavigation.Pedido = null;
                }

                await _context.Entry(pedido.IdvendaNavigation).Reference(e => e.IdclienteNavigation)
                    .Query()
                    .Include(e => e.IdenderecoNavigation)
                    .LoadAsync();
                await _context.Entry(pedido.IdvendaNavigation).Collection(e => e.ItemVenda).LoadAsync();

            });

            return pedidos;
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id, bool itemVenda = false, bool pagamento = false)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            await _context.Entry(pedido).Reference(e => e.IdentregadorNavigation)
                .LoadAsync();

            await _context.Entry(pedido).Reference(e => e.IdvendaNavigation)
                .Query()
                .Include(e => e.IdresponsavelNavigation)
                .Include(e => e.IdclienteNavigation)
                    .ThenInclude(e => e.IdenderecoNavigation)
                .LoadAsync();

            if(pedido.Delivery == 1)
            {
                await _context.Entry(pedido).Reference(e => e.IdmetodoNavigation)
                    .LoadAsync();

                await _context.Entry(pedido).Reference(e => e.IdtipoEntregadorNavigation)
                    .LoadAsync();
            }


            if (itemVenda)
            {
                await _context.Entry(pedido).Reference(e => e.IdvendaNavigation)
                    .Query()
                    .Include(e => e.ItemVenda)
                        .ThenInclude(e => e.IditemNavigation)
                    .Include(e => e.IdresponsavelNavigation)
                    .Include(e => e.IdclienteNavigation)
                        .ThenInclude(e => e.IdenderecoNavigation)
                    .LoadAsync();
            }

            if (pagamento)
            {
                await _context.Entry(pedido.IdvendaNavigation)
                .Collection(e => e.Pagamento)
                .LoadAsync();

                if (pedido.IdvendaNavigation.Pagamento != null)
                {
                    if (pedido.IdvendaNavigation.Pagamento.Count > 0)
                    {
                        foreach (var pag in pedido.IdvendaNavigation.Pagamento)
                        {
                            await _context.Entry(pag)
                                .Reference(e => e.IdmovimentoNavigation)
                                .LoadAsync();

                            await _context.Entry(pag.IdmovimentoNavigation)
                                .Reference(e => e.IdformaPagamentoNavigation)
                                .LoadAsync();
                        }
                    }
                }
            }


            return pedido;
        }

        // PUT: api/Pedidos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.Idvenda)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.NumeroPedido = await Pedido.GetLastNumero(_context);

            if(pedido.Idvenda == default & pedido.IdvendaNavigation != null)
            {
                pedido.IdvendaNavigation.NumeroVenda = await Venda.GetLastNumero(_context);
            }

            _context.Pedido.Add(pedido);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PedidoExists(pedido.Idvenda))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPedido", new { id = pedido.Idvenda }, pedido);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pedido>> DeletePedido(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Idvenda == id);
        }


        [HttpGet("{id}/taxaentrega")]
        public async Task<ActionResult<ItemVenda>> GetTaxaAplicavel(int id)
        {
            var venda = await _context.Venda.FindAsync(id);

            if (venda == null)
            {
                return NotFound();
            }

            await _context.Entry(venda)
                .Reference(e => e.IdclienteNavigation)
                .LoadAsync();

            if(venda.IdclienteNavigation == null)
            {
                return NotFound();
            }

            if(await venda.IdclienteNavigation.LoadEnderecoPrincipal(_context))
            {
                return await venda.IdclienteNavigation.EnderecoPrincipal.GetTaxaAplicavel(_context);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
