using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class EstoqueHasFornecedor
    {
        public int Idestoque { get; set; }
        public int Idfornecedor { get; set; }

        public virtual Estoque IdestoqueNavigation { get; set; }
        public virtual Fornecedor IdfornecedorNavigation { get; set; }
    }
}
