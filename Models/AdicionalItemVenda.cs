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
        public int Idadicional { get; set; }
        public int IditemVenda { get; set; }

        public virtual Adicional IdadicionalNavigation { get; set; }
        public virtual ItemVenda IditemVendaNavigation { get; set; }
    }
}
