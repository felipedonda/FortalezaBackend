using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class Venda
    {
        [NotMapped]
        public decimal ValorTotal { 
            get
            {
                return ItemVenda.Sum(e => e.ValorTotal);
            }
        }

        public async Task FecharVenda(fortalezaitdbContext dbcontext)
        {
            await dbcontext.Entry(this).Collection(e => e.ItemVenda).LoadAsync();

            foreach (var item in ItemVenda.ToList())
            {
                await item.MovimentarEstoqueVenda(dbcontext);
            }

            Aberta = 0;
            HoraFechamento = DateTime.UtcNow;

            dbcontext.Entry(this).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync();
        }

        public async Task RecontarIndices(fortalezaitdbContext dbcontext)
        {
            await dbcontext.Entry(this).Collection(e => e.ItemVenda).LoadAsync();

            if(ItemVenda != null)
            {
                if(ItemVenda.Count > 0 )
                {
                    int i = 1;
                    foreach (var e in ItemVenda.OrderBy(e => e.Indice))
                    {
                        e.Indice = i;
                        dbcontext.Entry(e).State = EntityState.Modified;
                        i++;
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> AddItemVenda(ItemVenda itemVenda, fortalezaitdbContext dbcontext)
        {
            if (itemVenda.IditemNavigation == null)
            {
                itemVenda.IditemNavigation = await dbcontext.Item.FindAsync(itemVenda.Iditem);

                if (itemVenda.IditemNavigation == null)
                {
                    return false;
                }
            }
            else
            {
                itemVenda.Iditem = itemVenda.IditemNavigation.Iditem;
            }

            itemVenda.Idvenda = Idvenda;

            await dbcontext.Entry(this).Collection(e => e.ItemVenda).LoadAsync();

            if(ItemVenda == null)
            {
                itemVenda.Indice = 1;
            }
            else
            {
                itemVenda.Indice = ItemVenda.Count + 1;
            }
            

            if(itemVenda.Valor == null)
            {
                await itemVenda.CalcularValor();
            }


            //Check for Combo and Adicionais

            itemVenda.IditemNavigation = null;
            itemVenda.Cancelado = 0;

            ItemVenda.Add(itemVenda);
            await dbcontext.SaveChangesAsync();

            dbcontext.Entry(this).State = EntityState.Modified;

            await dbcontext.SaveChangesAsync();
            return true;
        }

        public static async Task<int> GetLastNumero(fortalezaitdbContext dbcontext)
        {
            int UltimoNumero = await dbcontext.Venda.OrderByDescending(e => e.Idvenda).Select(e => e.NumeroVenda).FirstOrDefaultAsync();
            return UltimoNumero + 1;
        }
    }
}
