using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace FortalezaServer.Models
{
    public partial class Item
    {
        [NotMapped]
        public Estoque EstoqueAtual { get; set; }

        public async Task LoadItemTipo(fortalezaitdbContext dbcontext)
        {
            switch (Tipo)
            {
                case "Pacote":
                    await dbcontext.Entry(this)
                        .Reference(e => e.PacoteIditemNavigation)
                        .Query()
                            .Include(s => s.IditemProdutoNavigation)
                        .LoadAsync();
                    break;
            }
        }

        public async Task LoadItemEstoqueAtual(fortalezaitdbContext dbcontext)
        {
            if(Estoque == 1)
            {
                switch (Tipo)
                {
                    case "Produto":
                        var estoqueDisponivel = await dbcontext.Entry(this)
                            .Collection(e => e.ItemHasEstoque)
                            .Query()
                                .Include(e => e.IdestoqueNavigation)
                                .Where(e => e.IdestoqueNavigation.Disponivel == 1)
                                .OrderBy(e => e.IdestoqueNavigation.HoraEntrada)
                            .ToListAsync();
                        EstoqueAtual = new Estoque();
                        if(estoqueDisponivel != null)
                        {
                            if(estoqueDisponivel.Count > 0)
                            {
                                EstoqueAtual.Custo = estoqueDisponivel.First().IdestoqueNavigation.Custo;
                                EstoqueAtual.QuantidadeDisponivel = estoqueDisponivel.Sum(e => e.IdestoqueNavigation.QuantidadeDisponivel);
                            }
                        }
                        break;
                    case "Pacote":
                        if (PacoteIditemNavigation == null)
                        {
                            await LoadItemTipo(dbcontext);
                        }
                        if(PacoteIditemNavigation != null)
                        {
                            await PacoteIditemNavigation.IditemProdutoNavigation.LoadItemEstoqueAtual(dbcontext);
                        }
                        
                        
                        if(PacoteIditemNavigation != null)
                        {
                            if(PacoteIditemNavigation.IditemProdutoNavigation.Estoque == 1)
                            {
                                EstoqueAtual = new Estoque
                                {
                                    Custo = PacoteIditemNavigation.IditemProdutoNavigation.EstoqueAtual.Custo * PacoteIditemNavigation.Quantidade,
                                    QuantidadeDisponivel = PacoteIditemNavigation.IditemProdutoNavigation.EstoqueAtual.QuantidadeDisponivel / PacoteIditemNavigation.Quantidade
                                };
                            }
                        }
                        break;
                }
            }
        }


        public async Task<List<decimal>> AddEstoque(Estoque estoque, fortalezaitdbContext dbcontext)
        {
            if (Estoque == 1)
            {
                switch (Tipo)
                {
                    case "Produto":
                        List<decimal> custos = new List<decimal>();

                        estoque.ItemHasEstoque.Add(new ItemHasEstoque {
                            Iditem = Iditem
                        });

                        if(estoque.Saida == 1)
                        {
                            custos = await AtualizarEstoqueDisponivel(estoque.Quantidade,dbcontext);
                        }

                        dbcontext.Add(estoque);
                        await dbcontext.SaveChangesAsync();
                        return custos;
                    case "Pacote":
                        if (PacoteIditemNavigation == null)
                        {
                            await LoadItemTipo(dbcontext);
                        }
                        Estoque estoqueProduto = estoque;
                        estoqueProduto.Quantidade *= PacoteIditemNavigation.Quantidade;
                        if(estoqueProduto.Saida == 0)
                        {
                            estoqueProduto.QuantidadeDisponivel *= PacoteIditemNavigation.Quantidade;
                            if(estoqueProduto.Custo != 0)
                            {
                                estoqueProduto.Custo /= PacoteIditemNavigation.Quantidade;
                            }
                        }
                        return await PacoteIditemNavigation.IditemProdutoNavigation.AddEstoque(estoqueProduto, dbcontext);
                }
            }
            return null;
        }

        public async Task<List<decimal>> AtualizarEstoqueDisponivel(
            decimal quantity,
            fortalezaitdbContext dbcontext)
        {
            if(Tipo != "Produto")
            {
                throw new Exception("Método disponível somente para Produtos.");
            }
            await LoadItemEstoqueAtual(dbcontext);
            if(ItemHasEstoque != null)
            {
                if(ItemHasEstoque.Count > 0)
                {
                    return await AtualizarEstoqueDisponivel(quantity, ItemHasEstoque.ToList(), dbcontext);
                }
            }
            return null;
        }

            public async Task<List<decimal>> AtualizarEstoqueDisponivel(
            decimal quantity,
            List<ItemHasEstoque> estoquesDisponiveis,
            fortalezaitdbContext dbcontext)
        {
            List<decimal> Custos = new List<decimal>();
            var estoqueDisponivel = estoquesDisponiveis.First().IdestoqueNavigation;
            if (estoqueDisponivel.QuantidadeDisponivel >= quantity)
            {
                estoqueDisponivel.QuantidadeDisponivel -= quantity;

                for (int i = 0; i < quantity; i++)
                {
                    Custos.Add(estoqueDisponivel.Custo ?? default);
                }

                if (estoqueDisponivel.QuantidadeDisponivel == 0)
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
                Custos.AddRange(await AtualizarEstoqueDisponivel(quantity, estoquesDisponiveis, dbcontext));
            }

            dbcontext.Entry(estoqueDisponivel).State = EntityState.Modified;
            await dbcontext.SaveChangesAsync();
            return Custos;
        }
    }
}
