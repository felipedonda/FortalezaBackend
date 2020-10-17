using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class Pedido
    {
        public static async Task<int> GetLastNumero(fortalezaitdbContext dbcontext)
        {
            int UltimoNumero = await dbcontext.Pedido.OrderByDescending(e => e.Idvenda).Select(e => e.NumeroPedido).FirstOrDefaultAsync();
            return UltimoNumero + 1;
        }
    }
}
