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
    public class ClientesController : ControllerBase
    {
        private readonly fortalezaitdbContext _context;

        public ClientesController(fortalezaitdbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente(string query = "")
        {
            if(string.IsNullOrEmpty(query))
            {
                return await _context.Cliente
                .Include(e => e.IdenderecoNavigation)
                .ToListAsync();
            }
            else
            {
                List<int> queryClientes = await _context.Cliente
                    .Where(e => e.Nome.Contains(query))
                    .Select(e => e.Idcliente).ToListAsync();

                queryClientes.AddRange(await _context.Cliente
                    .Where(e => e.Cpf.Contains(query))
                    .Select(e => e.Idcliente).ToListAsync());

                queryClientes.AddRange(await _context.Cliente
                    .Where(e => e.Telefone.Contains(query))
                    .Select(e => e.Idcliente).ToListAsync());
                
                if(queryClientes.Count > 0)
                {
                    List<int> sortedClientes = queryClientes.GroupBy(e => e).Select(e => e.Key).ToList();
                    List<Cliente> queryResult = new List<Cliente>();
                    foreach(int idcliente in sortedClientes)
                    {
                        queryResult.Add(await _context.Cliente.FindAsync(idcliente));
                    }
                    return queryResult;
                }
                else
                {
                    return null;
                }
            }
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id, bool movimentos = false)
        {

            IQueryable<Cliente> clienteQuery;

            clienteQuery = _context.Cliente.Where(e => e.Idcliente == id);

            clienteQuery = clienteQuery.Include(e => e.IdenderecoNavigation);

            if (movimentos)
            {
                clienteQuery = clienteQuery
                    .Include(e => e.ClienteHasMovimento)
                    .ThenInclude(e => e.IdmovimentoNavigation);
            }

            Cliente cliente = await clienteQuery.FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // GET: api/Clientes/cpf/5
        [HttpGet("cpf/{id}")]
        public async Task<ActionResult<Cliente>> GetClienteByCpf(string cpf, bool movimentos = false)
        {

            IQueryable<Cliente> clienteQuery;

            clienteQuery = _context.Cliente.Where(e => e.Cpf == cpf);

            clienteQuery = clienteQuery.Include(e => e.IdenderecoNavigation);

            if (movimentos)
            {
                clienteQuery = clienteQuery
                    .Include(e => e.ClienteHasMovimento)
                    .ThenInclude(e => e.IdmovimentoNavigation);
            }

            Cliente cliente = await clienteQuery.FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Idcliente)
            {
                return BadRequest();
            }

            Endereco endereco = cliente.IdenderecoNavigation;
            _context.Entry(cliente).State = EntityState.Modified;
            _context.Entry(endereco).State = EntityState.Modified;

            //_context.Entry(cliente.IdenderecoNavigation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        [HttpPost("{id}/movimento")]
        public async Task<IActionResult> PostMovimento(int id, [FromBody] Movimento movimento)
        {
            Cliente cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            //Removendo as Navigations para evitar duplicação no banco de dados
            movimento.HoraEntrada = DateTime.Now;
            movimento.Idusuario = movimento.IdusuarioNavigation.Idusuario;
            movimento.IdusuarioNavigation = null;
            movimento.Idpdv = movimento.IdpdvNavigation.Idpdv;
            movimento.IdpdvNavigation = null;
            if(movimento.IdformaPagamentoNavigation != null)
            {
                movimento.IdformaPagamento = movimento.IdformaPagamentoNavigation.IdformaPagamento;
                movimento.IdformaPagamentoNavigation = null;
            }
            if(movimento.IdbandeiraNavigation != null)
            {
                movimento.Idbandeira = movimento.IdbandeiraNavigation.Idbandeira;
                movimento.IdbandeiraNavigation = null;
            }

            await cliente.AddMovimento(_context, movimento);

            return NoContent();
        }

        // POST: api/Clientes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Idcliente }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Idcliente == id);
        }
    }
}
