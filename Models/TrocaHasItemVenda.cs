using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class TrocaHasItemVenda
    {
        public int IditemVenda { get; set; }
        public int Idvenda { get; set; }
        public int Indice { get; set; }
        public decimal Quantidade { get; set; }

        public virtual ItemVenda IditemVendaNavigation { get; set; }
        public virtual Troca IdvendaNavigation { get; set; }
    }
}
