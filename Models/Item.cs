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
        public Item()
        {
            Adicional = new HashSet<Adicional>();
            ItemHasEstoque = new HashSet<ItemHasEstoque>();
            ItemHasGrupo = new HashSet<ItemHasGrupo>();
            ItemVenda = new HashSet<ItemVenda>();
            PacoteParentNavigation = new HashSet<Pacote>();
        }

        public int Iditem { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public string Imagem { get; set; }
        public string Unidade { get; set; }
        public byte Visivel { get; set; }
        public byte Disponivel { get; set; }
        public byte Estoque { get; set; }
        public decimal? EstoqueMinimo { get; set; }
        public string Tipo { get; set; }
        public byte PermiteCombo { get; set; }
        public byte UnidadeInteira { get; set; }

        [NotMapped]
        public Estoque EstoqueAtual { get; set; }

        public virtual Fiscal Fiscal { get; set; }
        public virtual Pacote PacoteNavigation { get; set; }
        public virtual ICollection<Adicional> Adicional { get; set; }
        public virtual ICollection<ItemHasEstoque> ItemHasEstoque { get; set; }
        public virtual ICollection<ItemHasGrupo> ItemHasGrupo { get; set; }
        public virtual ICollection<ItemVenda> ItemVenda { get; set; }
        public virtual ICollection<Pacote> PacoteParentNavigation { get; set; }


        public async Task LoadItemTipo(fortalezaitdbContext dbcontext)
        {
            switch (Tipo)
            {
                case "Pacote":
                    await dbcontext.Entry(this)
                        .Reference(e => e.PacoteNavigation)
                        .Query()
                            .Include(s => s.ProdutoNavigation)
                        .LoadAsync();
                    break;
            }
        }

        public async Task LoadItemEstoqueAtual(fortalezaitdbContext dbcontext)
        {
            switch (Tipo)
            {
                case "Produto":
                    var estoqueDisponivel = await dbcontext.Entry(this)
                        .Collection(e => e.ItemHasEstoque)
                        .Query()
                            .Include(e => e.EstoqueNavigation)
                            .Where(e => e.EstoqueNavigation.Disponivel == 1)
                            .OrderBy(e => e.EstoqueNavigation.HoraEntrada)
                        .ToListAsync();
                    EstoqueAtual = new Estoque
                    {
                        Custo = estoqueDisponivel.First().EstoqueNavigation.Custo,
                        QuantidadeDisponivel = estoqueDisponivel.Sum(e => e.EstoqueNavigation.QuantidadeDisponivel)
                    };
                    break;
                case "Pacote":
                    if (PacoteNavigation == null)
                    {
                        await LoadItemTipo(dbcontext);
                    }
                    await PacoteNavigation.ProdutoNavigation.LoadItemEstoqueAtual(dbcontext);
                    EstoqueAtual = new Estoque
                    {
                        Custo = PacoteNavigation.ProdutoNavigation.EstoqueAtual.Custo * PacoteNavigation.Quantidade,
                        QuantidadeDisponivel = PacoteNavigation.ProdutoNavigation.EstoqueAtual.QuantidadeDisponivel / PacoteNavigation.Quantidade
                    };
                    break;
            }
        }

    }
}
