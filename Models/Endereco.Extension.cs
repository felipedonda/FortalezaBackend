using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class Endereco
    {
        public async Task<ItemVenda> GetTaxaAplicavel(fortalezaitdbContext dbcontext)
        {
            Preferencias preferencias = await dbcontext.Preferencias.FindAsync(1);
            if(preferencias.ModoTaxaEntrega == 1)
            {
                TaxasEntrega taxasEntrega = await dbcontext.TaxasEntrega
                    .Where(e => e.Tipo == 1)
                    .Where(e => e.Bairro == Bairro)
                    .FirstOrDefaultAsync();
                if(taxasEntrega != null)
                {
                    return new ItemVenda
                    {
                        Custo = 0,
                        Iditem = preferencias.CodigoTaxaEntrega,
                        Quantidade = 1,
                        Valor = taxasEntrega.Taxa ?? default
                    };
                }
                else
                {
                    return new ItemVenda
                    {
                        Custo = 0,
                        Iditem = preferencias.CodigoTaxaEntrega,
                        Quantidade = 1,
                        Valor = preferencias.TaxaEntregaPadrao
                    };
                }

            }

            if(preferencias.ModoTaxaEntrega == 2)
            {
            }

            return null;
        }
    }
}
