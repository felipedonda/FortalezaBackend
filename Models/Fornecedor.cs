using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Fornecedor
    {
        public Fornecedor()
        {
            EstoqueHasFornecedor = new HashSet<EstoqueHasFornecedor>();
        }

        public int Idfornecedor { get; set; }

        public virtual ICollection<EstoqueHasFornecedor> EstoqueHasFornecedor { get; set; }
    }
}
