using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ItemVenda
    {
        public ItemVenda()
        {
            AdicionalItemVenda = new HashSet<AdicionalItemVenda>();
            TrocaHasItemVenda = new HashSet<TrocaHasItemVenda>();
        }

        public int IditemVenda { get; set; }
        public decimal Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Custo { get; set; }
        public decimal? Acrescimo { get; set; }
        public decimal? Desconto { get; set; }
        public decimal Cancelado { get; set; }
        public int Indice { get; set; }
        public int Idvenda { get; set; }
        public int Iditem { get; set; }

        public virtual Item IditemNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
        public virtual ICollection<AdicionalItemVenda> AdicionalItemVenda { get; set; }
        public virtual ICollection<TrocaHasItemVenda> TrocaHasItemVenda { get; set; }
    }
}
