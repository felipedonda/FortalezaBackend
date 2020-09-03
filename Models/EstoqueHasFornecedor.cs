using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class EstoqueHasFornecedor
    {
        public int EstoqueIdestoque { get; set; }
        public int FornecedorIdfornecedor { get; set; }

        public virtual Estoque EstoqueIdestoqueNavigation { get; set; }
        public virtual Fornecedor FornecedorIdfornecedorNavigation { get; set; }
    }
}
