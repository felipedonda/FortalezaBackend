using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class AdicionalHasEstoque
    {
        public int AdicionalIdadicional { get; set; }
        public int AdicionalItemIditem { get; set; }
        public int EstoqueIdestoque { get; set; }

        public virtual Adicional AdicionalI { get; set; }
        public virtual Estoque EstoqueIdestoqueNavigation { get; set; }
    }
}
