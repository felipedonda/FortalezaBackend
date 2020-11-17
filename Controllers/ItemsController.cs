using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FortalezaServer.Models;
using System.IO;

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
        public async Task<ActionResult<IEnumerable<Item>>> GetItem
            (
                bool estoqueAtual = false,
                bool estoqueOnly = false,
                bool visivelOnly = true
            )
        {
            List<Item> items = null;

            if (estoqueOnly)
            {
                items = await _context.Item
                    .Where(e => e.Estoque == 1)
                    .ToListAsync();
            }

            if(items == null & visivelOnly)
            {
                items = await _context.Item.Where(e => e.Visivel == 1).ToListAsync();
            }
            
            if(items == null)
            {
                items = await _context.Item.ToListAsync();
            }

            if(estoqueAtual)
            {
                items.ForEach(async e => await e.LoadItemEstoqueAtual(_context));
            }

            return items;
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
                        .Include(s => s.IdgrupoNavigation)
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
                        .Include(e => e.IdestoqueNavigation)
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
                .Where(e => !item.ItemHasGrupo.Any(s => s.Idgrupo == e.Idgrupo))
                .ToList()
                .ForEach(e => _item.ItemHasGrupo.Remove(e));

            item.ItemHasGrupo
                .Where(e => !CurrentGrupos.Any(s => s.Idgrupo == e.Idgrupo))
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

            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                if (item.PacoteIditemNavigation != null)
                {
                    _context.Entry(item.PacoteIditemNavigation).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
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
            if(item.ItemHasGrupo != null)
            {
                item.ItemHasGrupo.ToList().ForEach(e => e.IdgrupoNavigation = null);
            }

            item.PacoteIditemNavigation.IditemProdutoNavigation = null;



            _context.Item.Add(item);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Iditem }, item);
        }

        [HttpPost("{id}/estoques")]
        public async Task<ActionResult<Item>> PostEstoque(int id, Estoque estoque)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await item.LoadItemTipo(_context);

            await item.AddEstoque(estoque,_context);

            return CreatedAtAction("GetItem", new { id = item.Iditem }, item);
        }

        [HttpPost("{id}/imagem")]
        public async Task<ActionResult<Item>> PostImagem(int id, IFormFile imagem)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var fileName = "imagem-item-" + item.Iditem + Path.GetExtension(imagem.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(fileStream);
            }

            item.Imagem = fileName;
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Iditem });
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
