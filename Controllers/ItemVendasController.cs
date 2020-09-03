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
    public class ItemVendasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public ItemVendasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/ItemVendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemVenda>>> GetItemVenda()
        {
            return await _context.ItemVenda.Include(e => e.ItemNavigation).ToListAsync();
        }

        // GET: api/ItemVendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemVenda>> GetItemVenda(int id)
        {
            var itemVenda = await _context.ItemVenda.FindAsync(id);

            if (itemVenda == null)
            {
                return NotFound();
            }

            await _context.Entry(itemVenda).Reference(e => e.ItemNavigation).LoadAsync();

            return itemVenda;
        }

        // PUT: api/ItemVendas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemVenda(int id, ItemVenda itemVenda)
        {
            if (id != itemVenda.IditemVenda)
            {
                return BadRequest();
            }

            _context.Entry(itemVenda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemVendaExists(id))
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

        // POST: api/ItemVendas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ItemVenda>> PostItemVenda(ItemVenda itemVenda)
        {
            _context.ItemVenda.Add(itemVenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemVenda", new { id = itemVenda.IditemVenda }, itemVenda);
        }

        // DELETE: api/ItemVendas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemVenda>> DeleteItemVenda(int id)
        {
            var itemVenda = await _context.ItemVenda.FindAsync(id);
            if (itemVenda == null)
            {
                return NotFound();
            }

            _context.ItemVenda.Remove(itemVenda);
            await _context.SaveChangesAsync();

            return itemVenda;
        }

        private bool ItemVendaExists(int id)
        {
            return _context.ItemVenda.Any(e => e.IditemVenda == id);
        }
    }
}
