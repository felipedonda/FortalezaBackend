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
    public class ItemsController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public ItemsController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(
                int id,
                bool grupos = false,
                bool tipo = false,
                bool estoqueAtual = false,
                bool estoque = false
            )
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            if(grupos)
            {
                await _context.Entry(item)
                    .Collection(e => e.ItemHasGrupo)
                    .Query()
                        .Include(s => s.GrupoNavigation)
                    .LoadAsync();
            }

            if (tipo)
            {
                await item.LoadItemTipo(_context);
            }

            if(estoqueAtual && item.Estoque == 1)
            {
                await item.LoadItemEstoqueAtual(_context);
            }


            if (estoque && item.Estoque == 1 && item.Tipo == "Produto")
            {
                await _context.Entry(item)
                    .Collection(e => e.ItemHasEstoque)
                    .Query()
                        .Include(e => e.EstoqueNavigation)
                    .LoadAsync();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Iditem)
            {
                return BadRequest();
            }

            var _item = await _context.Item.FindAsync(id);

            var CurrentGrupos = await _context.Entry(_item).Collection(e => e.ItemHasGrupo).Query().ToListAsync();

            CurrentGrupos
                .Where(e => !item.ItemHasGrupo.Any(s => s.GrupoIdgrupo == e.GrupoIdgrupo))
                .ToList()
                .ForEach(e => _item.ItemHasGrupo.Remove(e));

            item.ItemHasGrupo
                .Where(e => !CurrentGrupos.Any(s => s.GrupoIdgrupo == e.GrupoIdgrupo))
                .ToList()
                .ForEach(e => _item.ItemHasGrupo.Add(e));

            try
            {
                await _context.SaveChangesAsync();
                _context.Entry(_item).State = EntityState.Detached;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            _context.Entry(item).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            item.ItemHasGrupo.ToList().ForEach(e => e.GrupoNavigation = null);
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Iditem }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Iditem == id);
        }
    }
}
