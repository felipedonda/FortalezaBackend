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
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Cliente
                .Include(e => e.ClienteHasEndereco)
                .ThenInclude(e => e.EnderecoIdenderecoNavigation)
                .ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            await _context.Entry(cliente)
                .Collection(e => e.ClienteHasEndereco)
                .Query()
                .Include(e => e.EnderecoIdenderecoNavigation)
                .LoadAsync();

            return cliente;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Idcliente)
            {
                return BadRequest();
            }

            var _cliente = await _context.Cliente.FindAsync(id);

            if (_cliente == null)
            {
                return NotFound();
            }

            await _context.Entry(_cliente)
                .Collection(e => e.ClienteHasEndereco)
                .Query()
                .Include(e => e.EnderecoIdenderecoNavigation)
                .LoadAsync();

            if(_cliente.ClienteHasEndereco.Count > 0)
            {
                foreach(var ce in _cliente.ClienteHasEndereco.ToList())
                {
                    _cliente.ClienteHasEndereco.Remove(ce);
                    _context.Endereco.Remove(ce.EnderecoIdenderecoNavigation);
                }
            }

            if(cliente.ClienteHasEndereco.Count > 0)
            {
                _cliente.ClienteHasEndereco.Add(cliente.ClienteHasEndereco.First());
            }

            await _context.SaveChangesAsync();

            _context.Entry(_cliente).State = EntityState.Detached;

            _context.Entry(cliente).State = EntityState.Modified;

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

        // POST: api/Clientes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
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
