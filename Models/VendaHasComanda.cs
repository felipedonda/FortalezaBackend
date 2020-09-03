using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class VendaHasComanda
    {
        public int VendaIdvenda { get; set; }
        public int ComandaIdcomanda { get; set; }

        public virtual Comanda ComandaIdcomandaNavigation { get; set; }
        public virtual Venda VendaIdvendaNavigation { get; set; }
    }
}
