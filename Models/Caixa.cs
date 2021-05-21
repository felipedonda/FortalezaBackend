using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Caixa
    {
        public Caixa()
        {
            Movimento = new HashSet<Movimento>();
            Venda = new HashSet<Venda>();
        }

        public int Idcaixa { get; set; }
        public byte Aberto { get; set; }
        public int IdnomeCaixa { get; set; }

        public virtual NomeCaixa IdnomeCaixaNavigation { get; set; }
        public virtual Abertura Abertura { get; set; }
        public virtual Fechamento Fechamento { get; set; }
        public virtual ICollection<Movimento> Movimento { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
