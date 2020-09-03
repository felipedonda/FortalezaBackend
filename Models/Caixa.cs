using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Caixa
    {
        public Caixa()
        {
            Movimento = new HashSet<Movimento>();
        }

        public int Idcaixa { get; set; }
        public string Nome { get; set; }
        public DateTime HoraAbertura { get; set; }
        public DateTime? HoraFechamento { get; set; }
        public decimal SaldoAbertura { get; set; }
        public decimal? SaldoFechamento { get; set; }
        public byte Aberto { get; set; }
        public int Idresponsavel { get; set; }

        public virtual Usuario IdresponsavelNavigation { get; set; }
        public virtual ICollection<Movimento> Movimento { get; set; }
    }
}
