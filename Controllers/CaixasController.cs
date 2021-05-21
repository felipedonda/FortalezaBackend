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
    public class CaixasController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public CaixasController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Caixas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caixa>>> GetCaixa(
            bool filtroData = false,
            string dataInicial = null,
            string dataFinal = null)
        {
            List<Caixa> caixas = new List<Caixa>();
            var query = _context.Caixa
                .Include(e => e.IdnomeCaixaNavigation)
                .Include(e => e.Abertura)
                    .ThenInclude(e => e.IdpdvNavigation)
                .Include(e => e.Abertura)
                    .ThenInclude(e => e.IdusuarioNavigation)
                .Include(e => e.Fechamento)
                    .ThenInclude(e => e.IdpdvNavigation)
                .Include(e => e.Fechamento)
                    .ThenInclude(e => e.IdusuarioNavigation)
                .AsQueryable();

            if (filtroData)
            {
                try
                {
                    DateTime _dataInicial = DateTime.ParseExact(dataInicial, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime _dataFinal = DateTime.ParseExact(dataFinal, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).AddDays(1).AddTicks(-1);
                    query = query.Where(e => (e.Abertura.Hora > _dataInicial) & (e.Abertura.Hora < _dataFinal));
                }
                catch
                {
                    return BadRequest();
                }
            }

            caixas = await query.ToListAsync();
            foreach(var caixa in caixas)
            {
                await _context.Entry(caixa)
                    .Reference(e => e.Abertura).LoadAsync();

                await _context.Entry(caixa)
                    .Reference(e => e.Fechamento).LoadAsync();

            }

            return caixas;
        }

        [HttpGet("actions/aberto")]
        public async Task<ActionResult<Caixa>> GetCaixaAberto(int idnomeCaixa, bool movimentos = false)
        {
            if(idnomeCaixa < 1)
            {
                return BadRequest();
            }

            var caixa = await _context.Caixa
                .Where(e => e.Aberto == 1 && e.IdnomeCaixa == idnomeCaixa)
                .Include(e => e.IdnomeCaixaNavigation)
                .Include(e => e.Abertura)
                    .ThenInclude(e => e.IdpdvNavigation)
                .Include(e => e.Abertura)
                    .ThenInclude(e => e.IdusuarioNavigation)
                .FirstOrDefaultAsync();

            if (caixa == null)
            {
                return NoContent();
            }

            if (movimentos)
            {
                await _context.Entry(caixa)
                    .Collection(e => e.Movimento)
                    .Query()
                    .Include(e => e.IdformaPagamentoNavigation)
                    .LoadAsync();
            }

            return caixa;
        }

        // GET: api/Caixas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Caixa>> GetCaixa(int id, bool movimentos = false)
        {
            var caixa = await _context.Caixa.FindAsync(id);

            await _context.Entry(caixa)
                .Reference(e => e.IdnomeCaixaNavigation)
                .LoadAsync();

            await _context.Entry(caixa)
                .Reference(e => e.Abertura)
                .LoadAsync();

            await _context.Entry(caixa)
                .Reference(e => e.Fechamento)
                .LoadAsync();

            if (caixa == null)
            {
                return NotFound();
            }

            if (movimentos)
            {
                await _context.Entry(caixa)
                    .Collection(e => e.Movimento)
                    .LoadAsync();
            }

            return caixa;
        }

        // PUT: api/Caixas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaixa(int id, Caixa caixa)
        {
            if (id != caixa.Idcaixa)
            {
                return BadRequest();
            }

            _context.Entry(caixa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                caixa.Aberto = 1;
                _context.Entry(caixa).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaixaExists(id))
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

        // POST: api/Caixas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Caixa>> PostCaixa(Caixa caixa)
        {
            caixa.IdnomeCaixa = caixa.IdnomeCaixaNavigation.IdnomeCaixa;
            caixa.IdnomeCaixaNavigation = null;
            _context.Caixa.Add(caixa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaixa", new { id = caixa.Idcaixa }, caixa);
        }

        // DELETE: api/Caixas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Caixa>> DeleteCaixa(int id)
        {
            var caixa = await _context.Caixa.FindAsync(id);
            if (caixa == null)
            {
                return NotFound();
            }

            _context.Caixa.Remove(caixa);
            await _context.SaveChangesAsync();

            return caixa;
        }


        [HttpPost("{id}/abrir")]
        public async Task<IActionResult> AbrirCaixa(
            int id,
            int idusuario,
            int idpdv)
        {
            var caixa = await _context.Caixa.FindAsync(id);

            if (caixa == null)
            {
                return NotFound();
            }

            Abertura abertura = new Abertura
            {
                Idpdv = idpdv,
                Idusuario = idusuario,
                Idcaixa = id,
                Hora = DateTime.Now
            };

            _context.Abertura.Add(abertura);
            await _context.SaveChangesAsync();
            
            caixa.Aberto = 1;
            _context.Entry(caixa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            

            return NoContent();
        }

        [HttpPost("{id}/fechar")]
        public async Task<IActionResult> FecharCaixa(
            int id,
            int idusuario,
            int idpdv)
        {
            var caixa = await _context.Caixa.FindAsync(id);

            if (caixa == null)
            {
                return NotFound();
            }

            Fechamento fechamento = new Fechamento
            {
                Idpdv = idpdv,
                Idusuario = idusuario,
                Idcaixa = id,
                Hora = DateTime.Now
            };

            _context.Fechamento.Add(fechamento);
            await _context.SaveChangesAsync();

            caixa.Aberto = 0;
            _context.Entry(caixa).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return NoContent();
        }


        private bool CaixaExists(int id)
        {
            return _context.Caixa.Any(e => e.Idcaixa == id);
        }
    }
}
