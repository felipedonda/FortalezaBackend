using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemHasAdicional = new HashSet<ItemHasAdicional>();
            ItemHasEstoque = new HashSet<ItemHasEstoque>();
            ItemHasGrupo = new HashSet<ItemHasGrupo>();
            ItemVenda = new HashSet<ItemVenda>();
            PacoteIditemProdutoNavigation = new HashSet<Pacote>();
        }

        public int Iditem { get; set; }
        public string Descricao { get; set; }
        public int? CodigoBarras { get; set; }
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
        public byte Varejo { get; set; }
        public decimal? ValorVarejo { get; set; }
        public decimal? QuantidadeVarejo { get; set; }

        public virtual Fiscal Fiscal { get; set; }
        public virtual Pacote PacoteIditemNavigation { get; set; }
        public virtual ICollection<ItemHasAdicional> ItemHasAdicional { get; set; }
        public virtual ICollection<ItemHasEstoque> ItemHasEstoque { get; set; }
        public virtual ICollection<ItemHasGrupo> ItemHasGrupo { get; set; }
        public virtual ICollection<ItemVenda> ItemVenda { get; set; }
        public virtual ICollection<Pacote> PacoteIditemProdutoNavigation { get; set; }
    }
}
