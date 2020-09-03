using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class FormaPagamento
    {
        public FormaPagamento()
        {
            Movimento = new HashSet<Movimento>();
        }

        public int IdformaPagamento { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public byte DebitarCliente { get; set; }
        public byte GerarContasReceber { get; set; }
        public byte Bandeira { get; set; }
        public byte Debito { get; set; }

        public virtual ICollection<Movimento> Movimento { get; set; }
    }
}
