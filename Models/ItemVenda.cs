using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class ItemVenda
    {
        public ItemVenda()
        {
            AdicionalItemVenda = new HashSet<AdicionalItemVenda>();
        }

        public int IditemVenda { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal? Custo { get; set; }
        public int ItemIditem { get; set; }
        public int VendaIdvenda { get; set; }
        public byte Cancelado { get; set; }
        public string Motivo { get; set; }

        public virtual Item ItemNavigation { get; set; }
        public virtual Venda VendaNavigation { get; set; }
        public virtual ICollection<AdicionalItemVenda> AdicionalItemVenda { get; set; }


        public async Task MovimentarEstoqueVenda(fortalezaitdbContext dbcontext)
        {
            await dbcontext.Entry(this).Reference(e => e.ItemNavigation).LoadAsync();
            if(ItemNavigation.Estoque == 1)
            {
                await ItemNavigation.LoadItemTipo(dbcontext);
                List<ItemHasEstoque> estoquesDisponiveis = new List<ItemHasEstoque>();
                List<decimal> Custos = new List<decimal>();
                List<IGrouping<decimal, decimal>> custosAgrupados = new List<IGrouping<decimal, decimal>>();
                Estoque estoque = new Estoque();

                switch (ItemNavigation.Tipo)
                {
                    case "Produto":
                        estoquesDisponiveis = await dbcontext.Entry(ItemNavigation).Collection(e => e.ItemHasEstoque)
                        .Query()
                            .Include(e => e.EstoqueNavigation)
                            .Where(e => e.EstoqueNavigation.Disponivel == 1)
                            .OrderBy(e => e.EstoqueNavigation.HoraEntrada)
                        .ToListAsync();

                        estoque = new Estoque
                        {
                            HoraEntrada = DateTime.UtcNow,
                            Custo = 0,
                            Disponivel = 0,
                            OrigemVenda = 1,
                            Saida = 1,
                            Quantidade = Quantidade,
                            QuantidadeDisponivel = 0
                        };

                        estoque.EstoqueHasVenda.Add(new EstoqueHasVenda
                        {
                            VendaIdvenda = VendaIdvenda
                        });

                        estoque.ItemHasEstoque.Add(new ItemHasEstoque
                        {
                            ItemIditem = ItemIditem
                        });

                        dbcontext.Add(estoque);

                        Custos = await MovimentarEstoqueVenda(Quantidade, estoquesDisponiveis, dbcontext);
                        custosAgrupados = Custos.GroupBy(e => e).ToList();
                        break;

                    case "Pacote":
                        estoquesDisponiveis = await dbcontext.Entry(ItemNavigation.PacoteNavigation.ProdutoNavigation).Collection(e => e.ItemHasEstoque)
                        .Query()
                            .Include(e => e.EstoqueNavigation)
                            .Where(e => e.EstoqueNavigation.Disponivel == 1)
                            .OrderBy(e => e.EstoqueNavigation.HoraEntrada)
                        .ToListAsync();


                        estoque = new Estoque
                        {
                            HoraEntrada = DateTime.UtcNow,
                            Custo = 0,
                            Disponivel = 0,
                            OrigemVenda = 1,
                            Saida = 1,
                            Quantidade = Quantidade * ItemNavigation.PacoteNavigation.Quantidade,
                            QuantidadeDisponivel = 0
                        };

                        estoque.EstoqueHasVenda.Add(new EstoqueHasVenda
                        {
                            VendaIdvenda = VendaIdvenda
                        });

                        estoque.ItemHasEstoque.Add(new ItemHasEstoque
                        {
                            ItemIditem = ItemNavigation.PacoteNavigation.IditemProduto
                        });

                        dbcontext.Add(estoque);

                        Custos = await MovimentarEstoqueVenda(Quantidade * ItemNavigation.PacoteNavigation.Quantidade, estoquesDisponiveis, dbcontext);

                        List<decimal> CustosPacote = new List<decimal>();
                        int ic = 0;
                        for (int i = 0; i < Quantidade; i++)
                        {
                            decimal custoPacote = 0;
                            for (int j = 0; j < ItemNavigation.PacoteNavigation.Quantidade; j++)
                            {
                                custoPacote += Custos[ic];
                                ic++;
                            }
                            CustosPacote.Add(custoPacote);
                        }

                        custosAgrupados = CustosPacote.GroupBy(e => e).ToList();
                        break;
                }

                Quantidade = custosAgrupados[0].Count();
                Custo = custosAgrupados[0].Key;

                dbcontext.Entry(this).State = EntityState.Modified;

                if (custosAgrupados.Count > 1)
                {
                    custosAgrupados.RemoveAt(0);
                    foreach (var custoAgrupado in custosAgrupados)
                    {
                        ItemVenda itemVenda = new ItemVenda
                        {
                            VendaIdvenda = VendaIdvenda,
                            ItemIditem = ItemIditem,
                            Custo = custoAgrupado.Key,
                            Quantidade = custoAgrupado.Count(),
                            Valor = Valor
                        };
                        dbcontext.ItemVenda.Add(itemVenda);
                    }
                }

                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<List<decimal>> MovimentarEstoqueVenda(
            decimal quantity,
            List<ItemHasEstoque> estoquesDisponiveis,
            fortalezaitdbContext dbcontext)
        {
            List<decimal> Custos = new List<decimal>();
            var estoqueDisponivel = estoquesDisponiveis.First().EstoqueNavigation;
            if (estoqueDisponivel.QuantidadeDisponivel >= quantity)
            {
                estoqueDisponivel.QuantidadeDisponivel -= quantity;
                
                for(int i = 0; i < quantity; i++)
                {
                    Custos.Add(estoqueDisponivel.Custo ?? default);
                }

                if(estoqueDisponivel.QuantidadeDisponivel == 0)
                {
                    estoqueDisponivel.Disponivel = 0;
                }

            }
            else
            {
                quantity -= estoqueDisponivel.QuantidadeDisponivel;

                for (int i = 0; i < estoqueDisponivel.QuantidadeDisponivel; i++)
                {
                    Custos.Add(estoqueDisponivel.Custo ?? default);
                }

                estoqueDisponivel.QuantidadeDisponivel = 0;
                estoqueDisponivel.Disponivel = 0;
                estoquesDisponiveis.RemoveAt(0);
                Custos.AddRange(await MovimentarEstoqueVenda(quantity, estoquesDisponiveis, dbcontext));
            }

            dbcontext.Entry(estoqueDisponivel).State = EntityState.Modified;
            return Custos;
        }
    }

}
