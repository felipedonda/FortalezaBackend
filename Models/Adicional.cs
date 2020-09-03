using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Adicional
    {
        public Adicional()
        {
            AdicionalHasEstoque = new HashSet<AdicionalHasEstoque>();
            AdicionalItemVenda = new HashSet<AdicionalItemVenda>();
        }

        public int Idadicional { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public string Unidade { get; set; }
        public byte Incluso { get; set; }
        public int ItemIditem { get; set; }

        public virtual Item ItemIditemNavigation { get; set; }
        public virtual ICollection<AdicionalHasEstoque> AdicionalHasEstoque { get; set; }
        public virtual ICollection<AdicionalItemVenda> AdicionalItemVenda { get; set; }
    }
}
