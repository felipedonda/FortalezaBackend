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
    public class InformacoesEmpresaController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public InformacoesEmpresaController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/InformacoesEmpresa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InformacoesEmpresa>>> GetInformacoesEmpresa()
        {
            return await _context.InformacoesEmpresa.ToListAsync();
        }

        // GET: api/InformacoesEmpresa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InformacoesEmpresa>> GetInformacoesEmpresa(int id)
        {
            var informacoesEmpresa = await _context.InformacoesEmpresa.FindAsync(id);

            if (informacoesEmpresa == null)
            {
                return NotFound();
            }
            else
            {
                await _context.Entry(informacoesEmpresa).Reference(e => e.IdenderecoNavigation).LoadAsync();
            }

            return informacoesEmpresa;
        }

        // PUT: api/InformacoesEmpresa/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInformacoesEmpresa(int id, InformacoesEmpresa informacoesEmpresa)
        {
            if (id != informacoesEmpresa.IdinformacoesEmpresa)
            {
                return BadRequest();
            }

            _context.Entry(informacoesEmpresa).State = EntityState.Modified;
            _context.Entry(informacoesEmpresa.IdenderecoNavigation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InformacoesEmpresaExists(id))
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

        // POST: api/InformacoesEmpresa
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<InformacoesEmpresa>> PostInformacoesEmpresa(InformacoesEmpresa informacoesEmpresa)
        {
            _context.InformacoesEmpresa.Add(informacoesEmpresa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInformacoesEmpresa", new { id = informacoesEmpresa.IdinformacoesEmpresa }, informacoesEmpresa);
        }

        [HttpPost("{id}/imagem")]
        public async Task<IActionResult> PostImagem(int id)
        {
            var informacoesEmpresa = await _context.InformacoesEmpresa.FindAsync(id);

            if (informacoesEmpresa == null)
            {
                return NotFound();
            }

            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                if(form.Files.Count > 0)
                {
                    var fileName = "logo-empresa" + Path.GetExtension(form.Files[0].FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await form.Files[0].CopyToAsync(fileStream);
                    }

                    informacoesEmpresa.Logo = fileName;
                    _context.Entry(informacoesEmpresa).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            return Content(informacoesEmpresa.Logo);
        }

        // DELETE: api/InformacoesEmpresa/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InformacoesEmpresa>> DeleteInformacoesEmpresa(int id)
        {
            var informacoesEmpresa = await _context.InformacoesEmpresa.FindAsync(id);
            if (informacoesEmpresa == null)
            {
                return NotFound();
            }

            _context.InformacoesEmpresa.Remove(informacoesEmpresa);
            await _context.SaveChangesAsync();

            return informacoesEmpresa;
        }

        private bool InformacoesEmpresaExists(int id)
        {
            return _context.InformacoesEmpresa.Any(e => e.IdinformacoesEmpresa == id);
        }
    }
}
