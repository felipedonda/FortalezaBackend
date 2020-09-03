using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ItemHasEstoque
    {
        public int ItemIditem { get; set; }
        public int EstoqueIdestoque { get; set; }

        public virtual Estoque EstoqueNavigation { get; set; }
        public virtual Item ItemNavigation { get; set; }
    }
}
