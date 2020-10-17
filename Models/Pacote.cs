using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pacote
    {
        public int Iditem { get; set; }
        public int IditemProduto { get; set; }
        public decimal Quantidade { get; set; }
        public byte Padrao { get; set; }

        public virtual Item IditemNavigation { get; set; }
        public virtual Item IditemProdutoNavigation { get; set; }
    }
}
