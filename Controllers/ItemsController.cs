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
                bool visivelOnly = true,
                string query = ""
            )
        {

            List<Item> queryResult = new List<Item>();

            if (string.IsNullOrEmpty(query))
            {

                var items = _context.Item.AsQueryable();

                if (estoqueOnly)
                {
                    items = items.Where(e => e.Estoque == 1);
                }

                if (visivelOnly)
                {
                    items = items.Where(e => e.Visivel == 1);
                }

                queryResult = await items.ToListAsync();
            }
            else
            {
                List<int> queryItems = await _context.Item
                    .Where(e => e.Descricao.Contains(query))
                    .Select(e => e.Iditem).ToListAsync();

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
                    queryItems.AddRange(await _context.Item
                        .Where(e => e.Iditem.ToString().Contains(query.Substring(leadingZeros)))
                        .Select(e => e.Iditem).ToListAsync());
                }

                queryItems.AddRange(await _context.Item
                    .Where(e => (ldzAux + e.Iditem.ToString()).Contains(query))
                    .Select(e => e.Iditem).ToListAsync());

                queryItems.AddRange(await _context.Item
                    .Where(e => e.CodigoBarras.Contains(query))
                    .Select(e => e.Iditem).ToListAsync()); ;

                if (queryItems.Count > 0)
                {
                    List<int> sortedItems = queryItems.GroupBy(e => e).OrderBy(e => e.Count()).Select(e => e.Key).ToList();
                    foreach (int iditem in sortedItems)
                    {
                        queryResult.Add(await _context.Item.FindAsync(iditem));
                    }
                    return queryResult;
                }
                else
                {
                    return null;
                }
            }

            if (estoqueAtual)
            {
                queryResult.ForEach(async e => await e.LoadItemEstoqueAtual(_context));
            }

            queryResult.ForEach(async e =>
                await _context.Entry(e)
                    .Reference(f => f.Fiscal)
                    .LoadAsync()
            );
                

            return queryResult;
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

            await _context.Entry(item)
                .Reference(e => e.Fiscal)
                .LoadAsync();

            if (item == null)
            {
                return NotFound();
            }

            if (grupos)
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

            if (estoqueAtual && item.Estoque == 1)
            {
                await item.LoadItemEstoqueAtual(_context);
            }


            if (estoque && item.Estoque == 1 && item.Tipo == 1)
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

            var _item = await _context.Item.Where(e => e.Iditem == id)
                .Include(e => e.Fiscal)
                .FirstOrDefaultAsync();

            bool hasPacote = false;
            bool hasFiscal = false;

            hasFiscal = _item.Fiscal != null;

            if (_item.Tipo == 2)
            {
                await _item.LoadItemTipo(_context);
                hasPacote = _item.Pacote != null;
            }

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

                if(item.Fiscal != null)
                {
                    if(hasFiscal)
                    {
                        item.Fiscal.Iditem = item.Iditem;
                        _context.Entry(item.Fiscal).State = EntityState.Modified;
                       
                    }
                    else
                    {
                        item.Fiscal.Iditem = item.Iditem;
                        item.Fiscal.IditemNavigation = null;
                        _context.Fiscal.Add(item.Fiscal);
                    }
                    await _context.SaveChangesAsync();
                }

                if (item.Tipo == 2)
                {
                    if (item.Pacote != null)
                    {
                        if (hasPacote)
                        {
                            item.Pacote.Iditem = item.Iditem;
                            _context.Entry(item.Pacote).State = EntityState.Modified;
                        }
                        else
                        {
                            item.Pacote.Iditem = item.Iditem;
                            item.Pacote.IditemNavigation = null;
                            item.Pacote.IditemProdutoNavigation = null;
                            _context.Pacote.Add(item.Pacote);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest();
                    }
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
            if (item.ItemHasGrupo != null)
            {
                item.ItemHasGrupo.ToList().ForEach(e => e.IdgrupoNavigation = null);
            }

            if(item.Pacote != null)
            {
                if(item.Tipo != 2)
                {
                    item.Pacote = null;
                }
                else
                {
                    if (item.Pacote.IditemProdutoNavigation != null)
                    {
                        item.Pacote.IditemProdutoNavigation = null;
                    }
                }
            }
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

            await item.AddEstoque(estoque, _context);

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

        // GET: api/Items/5/exists
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> ItemExistsRoute(int id)
        {
            return await _context.Item.AnyAsync(e => e.Iditem == id);
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Iditem == id);
        }
    }
}
