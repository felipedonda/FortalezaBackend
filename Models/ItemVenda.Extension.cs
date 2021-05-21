using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class ItemVenda
    {
        [NotMapped]
        public decimal ValorTotal { get { return Quantidade * Valor ?? default; } }

        public async Task CalcularValor()
        {
            bool valorAtacado = false;

            if (IditemNavigation.Atacado == 1)
            {
                if (IditemNavigation.QuantidadeAtacado != null)
                {
                    if (IditemNavigation.QuantidadeAtacado <= Quantidade)
                    {
                        valorAtacado = true;
                    }
                }
            }

            if(valorAtacado)
            {
                Valor = IditemNavigation.ValorAtacado;
            }
            else
            {
                Valor = IditemNavigation.Valor;
            }
        }

        public async Task MovimentarEstoqueVenda(fortalezaitdbContext dbcontext)
        {
            await dbcontext.Entry(this).Reference(e => e.IditemNavigation).LoadAsync();
            if (IditemNavigation.Estoque == 1)
            {
                await IditemNavigation.LoadItemTipo(dbcontext);
                List<decimal> Custos = new List<decimal>();
                List<IGrouping<decimal, decimal>> custosAgrupados = new List<IGrouping<decimal, decimal>>();
                Estoque estoque = new Estoque();

                switch (IditemNavigation.Tipo)
                {
                    case 1:

                        estoque = new Estoque
                        {
                            HoraEntrada = DateTime.Now,
                            Custo = 0,
                            Disponivel = 0,
                            OrigemVenda = 1,
                            Saida = 1,
                            Quantidade = Quantidade,
                            QuantidadeDisponivel = 0
                        };

                        estoque.EstoqueHasVenda.Add(new EstoqueHasVenda
                        {
                            Idvenda = Idvenda
                        });

                        Custos = await IditemNavigation.AddEstoque(estoque, dbcontext);
                        if(Custos != null)
                        {
                            custosAgrupados = Custos.GroupBy(e => e).ToList();
                        }
                        break;

                    case 2:

                        estoque = new Estoque
                        {
                            HoraEntrada = DateTime.Now,
                            Custo = 0,
                            Disponivel = 0,
                            OrigemVenda = 1,
                            Saida = 1,
                            Quantidade = Quantidade,
                            QuantidadeDisponivel = 0
                        };

                        estoque.EstoqueHasVenda.Add(new EstoqueHasVenda
                        {
                            Idvenda = Idvenda
                        });

                        Custos = await IditemNavigation
                            .AddEstoque(
                                estoque,
                                dbcontext
                            );

                        List<decimal> CustosPacote = new List<decimal>();

                        if (Custo != null)
                        {
                            int ic = 0;
                            for (int i = 0; i < Quantidade; i++)
                            {
                                decimal custoPacote = 0;
                                for (int j = 0; j < IditemNavigation.Pacote.Quantidade; j++)
                                {
                                    custoPacote += Custos[ic];
                                    ic++;
                                }
                                CustosPacote.Add(custoPacote);
                            }
                            custosAgrupados = CustosPacote.GroupBy(e => e).ToList();
                        }
                        break;
                }

                if(custosAgrupados.Count > 0)
                {
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
                                Idvenda = Idvenda,
                                Iditem = Iditem,
                                Indice = Indice,
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
        }
    }
}
