using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pacote
    {
        public int ItemIditem { get; set; }
        public int IditemProduto { get; set; }
        public decimal Quantidade { get; set; }
        public byte Padrao { get; set; }

        public virtual Item ProdutoNavigation { get; set; }
        public virtual Item ItemParentNavigation { get; set; }
    }
}
