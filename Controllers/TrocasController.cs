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
    public class TrocasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public TrocasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Trocas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Troca>>> GetTroca()
        {
            return await _context.Troca.ToListAsync();
        }

        // GET: api/Trocas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Troca>> GetTroca(int id)
        {
            var troca = await _context.Troca.FindAsync(id);

            if (troca == null)
            {
                return NotFound();
            }

            return troca;
        }

        // PUT: api/Trocas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTroca(int id, Troca troca)
        {
            if (id != troca.Idvenda)
            {
                return BadRequest();
            }

            _context.Entry(troca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrocaExists(id))
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

        // POST: api/Trocas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Troca>> PostTroca(Troca troca)
        {
            troca.IdvendaNavigation = null;
            troca.IdpdvNavigation = null;
            troca.IdusuarioNavigation = null;
            foreach(var ItemTroca in troca.TrocaHasItemVenda)
            {
                ItemTroca.IditemVendaNavigation = null;
                ItemTroca.IdvendaNavigation = null;
            }
            _context.Troca.Add(troca);

            try
            {
                await _context.SaveChangesAsync();
                Venda venda = await _context.Venda.FindAsync(troca.Idvenda);
                await _context.Entry(venda).Collection(e => e.ItemVenda).LoadAsync();
                await _context.Entry(venda).Reference(e => e.IdclienteNavigation).LoadAsync();

                foreach (var itemTroca in troca.TrocaHasItemVenda)
                {
                    foreach (var itemVenda in venda.ItemVenda)
                    {
                        if (itemVenda.IditemVenda == itemTroca.IditemVenda)
                        {
                            itemVenda.Cancelado = itemTroca.Quantidade;
                            _context.Entry(itemVenda).State = EntityState.Modified;
                        }
                    }
                }
                await _context.SaveChangesAsync();

                await venda.IdclienteNavigation.AddMovimento(_context, new Movimento
                {
                    HoraEntrada = DateTime.Now,
                    Idpdv = troca.Idpdv,
                    Idusuario = troca.Idusuario,
                    Valor = troca.ValorTotal,
                    Tipo = 2,
                    Descricao = "CRÉDITO DE TROCA"
                });
            }
            catch (DbUpdateException)
            {
                if (TrocaExists(troca.Idvenda))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTroca", new { id = troca.Idvenda }, troca);
        }

        // DELETE: api/Trocas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Troca>> DeleteTroca(int id)
        {
            var troca = await _context.Troca.FindAsync(id);
            if (troca == null)
            {
                return NotFound();
            }

            _context.Troca.Remove(troca);
            await _context.SaveChangesAsync();

            return troca;
        }

        private bool TrocaExists(int id)
        {
            return _context.Troca.Any(e => e.Idvenda == id);
        }
    }
}
