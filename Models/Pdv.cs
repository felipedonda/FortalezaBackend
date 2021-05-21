using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pdv
    {
        public Pdv()
        {
            Abertura = new HashSet<Abertura>();
            Fechamento = new HashSet<Fechamento>();
            Movimento = new HashSet<Movimento>();
            Troca = new HashSet<Troca>();
            Venda = new HashSet<Venda>();
        }

        public int Idpdv { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Abertura> Abertura { get; set; }
        public virtual ICollection<Fechamento> Fechamento { get; set; }
        public virtual ICollection<Movimento> Movimento { get; set; }
        public virtual ICollection<Troca> Troca { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
