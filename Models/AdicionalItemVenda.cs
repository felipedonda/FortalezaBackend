using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class AdicionalItemVenda
    {
        public int IdadicionalVenda { get; set; }
        public int AdicionalIdadicional { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal? Custo { get; set; }
        public int AdicionalIdadicional1 { get; set; }
        public int AdicionalItemIditem { get; set; }
        public int ItemVendaIditemVenda { get; set; }
        public int ItemVendaItemIditem { get; set; }
        public int ItemVendaVendaIdvenda { get; set; }

        public virtual Adicional AdicionalI { get; set; }
        public virtual ItemVenda ItemVenda { get; set; }
    }
}
