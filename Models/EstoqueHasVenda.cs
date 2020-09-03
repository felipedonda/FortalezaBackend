using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class EstoqueHasVenda
    {
        public int EstoqueIdestoque { get; set; }
        public int VendaIdvenda { get; set; }

        public virtual Estoque EstoqueIdestoqueNavigation { get; set; }
        public virtual Venda VendaIdvendaNavigation { get; set; }
    }
}
