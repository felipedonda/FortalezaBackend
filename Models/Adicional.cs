using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Adicional
    {
        public Adicional()
        {
            AdicionalItemVenda = new HashSet<AdicionalItemVenda>();
            ItemHasAdicional = new HashSet<ItemHasAdicional>();
        }

        public int Idadicional { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public string Unidade { get; set; }
        public byte Incluso { get; set; }

        public virtual ICollection<AdicionalItemVenda> AdicionalItemVenda { get; set; }
        public virtual ICollection<ItemHasAdicional> ItemHasAdicional { get; set; }
    }
}
