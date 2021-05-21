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
                case 2:
                    await dbcontext.Entry(this)
                        .Reference(e => e.Pacote)
                        .Query()
                            .Include(s => s.IditemProdutoNavigation)
                        .LoadAsync();
                    break;
            }
        }

        public async Task LoadItemEstoqueAtual(fortalezaitdbContext dbcontext)
        {
            switch (Tipo)
            {
                case 1:
                    if (Estoque == 1)
                    {
                        var estoqueDisponivel = await dbcontext.Entry(this)
                            .Collection(e => e.ItemHasEstoque)
                            .Query()
                                .Include(e => e.IdestoqueNavigation)
                                .Where(e => e.IdestoqueNavigation.Disponivel == 1)
                                .OrderBy(e => e.IdestoqueNavigation.HoraEntrada)
                            .ToListAsync();
                        EstoqueAtual = new Estoque();
                        if (estoqueDisponivel != null)
                        {
                            if (estoqueDisponivel.Count > 0)
                            {
                                EstoqueAtual.Custo = estoqueDisponivel.First().IdestoqueNavigation.Custo;
                                EstoqueAtual.QuantidadeDisponivel = estoqueDisponivel.Sum(e => e.IdestoqueNavigation.QuantidadeDisponivel);
                            }
                        }
                        
                    }
                    break;
                case 2:
                    if (Pacote == null)
                    {
                        await LoadItemTipo(dbcontext);
                    }
                    if (Pacote != null)
                    {
                        if (Pacote.IditemProdutoNavigation.Estoque == 1)
                        {
                            await Pacote.IditemProdutoNavigation.LoadItemEstoqueAtual(dbcontext);
                            EstoqueAtual = new Estoque
                            {
                                Custo = Pacote.IditemProdutoNavigation.EstoqueAtual.Custo * Pacote.Quantidade,
                                QuantidadeDisponivel = Pacote.IditemProdutoNavigation.EstoqueAtual.QuantidadeDisponivel / Pacote.Quantidade
                            };
                        }
                    }
                    break;
            }
        }


        public async Task<List<decimal>> AddEstoque(Estoque estoque, fortalezaitdbContext dbcontext)
        {
            if (Estoque == 1)
            {
                switch (Tipo)
                {
                    case 1:
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
                    case 2:
                        if (Pacote == null)
                        {
                            await LoadItemTipo(dbcontext);
                        }
                        Estoque estoqueProduto = estoque;
                        estoqueProduto.Quantidade *= Pacote.Quantidade;
                        if(estoqueProduto.Saida == 0)
                        {
                            estoqueProduto.QuantidadeDisponivel *= Pacote.Quantidade;
                            if(estoqueProduto.Custo != 0)
                            {
                                estoqueProduto.Custo /= Pacote.Quantidade;
                            }
                        }
                        return await Pacote.IditemProdutoNavigation.AddEstoque(estoqueProduto, dbcontext);
                }
            }
            return null;
        }

        public async Task<List<decimal>> AtualizarEstoqueDisponivel(
            decimal quantity,
            fortalezaitdbContext dbcontext)
        {
            if(Tipo != 1)
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

                dbcontext.Entry(estoqueDisponivel).State = EntityState.Modified;
                await dbcontext.SaveChangesAsync();
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

                dbcontext.Entry(estoqueDisponivel).State = EntityState.Modified;
                await dbcontext.SaveChangesAsync();

                if (estoquesDisponiveis.Count > 0)
                {
                    Custos.AddRange(await AtualizarEstoqueDisponivel(quantity, estoquesDisponiveis, dbcontext));
                }
            }
            return Custos;
        }
    }
}
