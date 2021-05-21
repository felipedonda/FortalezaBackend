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
                int tipo = -1,
                bool abertas = false,
                bool filtroData = false,
                string dataInicial = null,
                string dataFinal = null,
                string query = ""
            )
        {

            if(string.IsNullOrWhiteSpace(query))
            {
                var vendasQuery = _context.Venda
                .Include(e => e.ItemVenda)
                .AsQueryable();

                if (tipo >= 0)
                {
                    vendasQuery = vendasQuery.Where(e => e.Tipo == tipo);
                }

                if (filtroData)
                {
                    try
                    {
                        DateTime _dataInicial = DateTime.ParseExact(dataInicial, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime _dataFinal = DateTime.ParseExact(dataFinal, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).AddDays(1).AddTicks(-1);
                        vendasQuery = vendasQuery.Where(e => e.HoraEntrada > _dataInicial & e.HoraEntrada < _dataFinal);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }


                if (abertas)
                {
                    vendasQuery = vendasQuery.Where(e => e.Aberta == 1);
                }


                var vendas = await vendasQuery.ToListAsync();

                return vendas;
            }
            else
            {
                List<int> queryVendas = await _context.Venda
                    .Include(e => e.IdclienteNavigation)
                    .Where(e => e.IdclienteNavigation.Nome.Contains(query))
                    .Select(e => e.Idvenda).ToListAsync();

                int leadingZeros = 0;
                string ldzAux = "";
                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i] == '0')
                    {
                        leadingZeros++;
                        ldzAux += '0';
                    }
                    else
                    {
                        i = query.Length;
                    }
                }

                if (query.Length != leadingZeros)
                {
                    queryVendas.AddRange(await _context.Venda
                        .Where(e => e.Idvenda.ToString().Contains(query.Substring(leadingZeros)))
                        .Select(e => e.Idvenda).ToListAsync());
                }

                queryVendas.AddRange(await _context.Venda
                    .Where(e => (ldzAux + e.Idvenda.ToString()).Contains(query))
                    .Select(e => e.Idvenda).ToListAsync());

                queryVendas.AddRange(await _context.Venda
                    .Where(e => e.NumeroVenda.ToString().Contains(query))
                    .Select(e => e.Idvenda).ToListAsync());

                List<Venda> queryResult = new List<Venda>();


                DateTime _dataInicial = new DateTime();
                DateTime _dataFinal = new DateTime();

                if (filtroData)
                {
                    _dataInicial = DateTime.ParseExact(dataInicial, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    _dataFinal = DateTime.ParseExact(dataFinal, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).AddDays(1).AddTicks(-1);
                }


                if (queryVendas.Count > 0)
                {
                    List<int> sortedItems = queryVendas.GroupBy(e => e).OrderBy(e => e.Count()).Select(e => e.Key).ToList();
                    foreach (int idvenda in sortedItems)
                    {
                        if(filtroData)
                        {
                            Venda venda = await _context.Venda
                                .Include(e => e.IdclienteNavigation)
                                .Where(e => e.Idvenda == idvenda)
                                .Where(e => e.HoraEntrada > _dataInicial & e.HoraEntrada < _dataFinal)
                                .FirstOrDefaultAsync();
                            if (venda != null)
                            {
                                queryResult.Add(venda);
                            }
                        }
                        else
                        {
                            queryResult.Add(await _context.Venda
                                .Include(e => e.IdclienteNavigation)
                                .Where(e => e.Idvenda == idvenda)
                                .FirstOrDefaultAsync());
                        }

                    }
                    return queryResult;
                }
                else
                {
                    return null;
                }
            }

        }

        // GET: api/Vendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetVenda
            (
                int id,
                bool itemvendas = false,
                bool pagamentos = false,
                bool troca = false
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

            if (troca)
            {
                await _context.Entry(venda).Reference(e => e.Troca)
                    .Query()
                    .Include(e => e.TrocaHasItemVenda)
                    .LoadAsync();
            }

            if (venda.Idcliente != null)
            {
                await _context.Entry(venda).Reference(e => e.IdclienteNavigation)
                    .Query()
                    .Include(e => e.IdenderecoNavigation)
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

        [HttpGet("actions/hasaberta")]
        public async Task<ActionResult<bool>> HasVendaAberta()
        {
            return await _context.Venda.AnyAsync(e => e.Aberta == 1);
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
            //registrando o número da venda. Registro separado do ID já que esse pode ser resetado.
            venda.NumeroVenda = await Venda.GetLastNumero(_context);

            _context.Venda.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenda", new { id = venda.Idvenda }, venda);
        }


        [HttpPost("{id}/pagamentos")]
        public async Task<ActionResult<Venda>> PostPagamento(int id, Pagamento pagamento)
        {
            var venda = await _context.Venda
                .Where(e => e.Idvenda == id)
                .Include(e => e.ItemVenda)
                .Include(e => e.IdclienteNavigation)
                .ThenInclude(e => e.ClienteHasMovimento)
                .ThenInclude(e => e.IdmovimentoNavigation)
                .FirstOrDefaultAsync();


            if (venda == null)
            {
                return NotFound();
            }

            //checando caso o pagamento debita do cliente e se o seu saldo é positivo.
            if (pagamento.IdmovimentoNavigation.IdformaPagamentoNavigation.DebitarCliente == 1)
            {
                if(venda.IdclienteNavigation != null)
                {
                    if((venda.IdclienteNavigation.SaldoEmConta >= pagamento.IdmovimentoNavigation.Valor) || pagamento.IdmovimentoNavigation.IdformaPagamentoNavigation.GerarContasReceber == 1)
                    {
                        //criando movimentação financeira na conta do cliente
                        await venda.IdclienteNavigation.AddMovimento(_context, new Movimento
                        {
                            HoraEntrada = DateTime.Now,
                            Idpdv = pagamento.IdmovimentoNavigation.Idpdv,
                            Idusuario = pagamento.IdmovimentoNavigation.Idusuario,
                            Valor = pagamento.IdmovimentoNavigation.Valor,
                            Tipo = 3,
                            Descricao = "COMPRA COD: " + venda.Idvenda
                        });
                    }
                    else
                    {
                        return Unauthorized("Saldo do cliente insuficiente para operação.");
                    }
                }
                else
                {
                    return Unauthorized("Cliente não selecionado.");
                }
            }

            //limpando as navigations para evitar que seja salvo em dobro
            pagamento.Idvenda = id;
            pagamento.IdmovimentoNavigation.IdformaPagamentoNavigation = null;
            pagamento.IdmovimentoNavigation.IdcaixaNavigation = null;

            venda.ValorPago += pagamento.IdmovimentoNavigation.Valor;

            if (venda.ValorPago >= venda.ValorTotal)
            {
                venda.Paga = 1;
            }

            if (venda.ValorPago > venda.ValorTotal)
            {
                pagamento.IdmovimentoNavigation.Valor -= venda.ValorPago - venda.ValorTotal;
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
