using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Movimento
    {
        public Movimento()
        {
            MovimentoHasBandeira = new HashSet<MovimentoHasBandeira>();
            Pagamento = new HashSet<Pagamento>();
        }

        public int Idmovimento { get; set; }
        public DateTime HoraEntrada { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public int FormaPagamentoIdformaPagamento { get; set; }
        public int CaixaIdcaixa { get; set; }

        public virtual Caixa CaixaIdcaixaNavigation { get; set; }
        public virtual FormaPagamento FormaPagamentoNavigation { get; set; }
        public virtual ICollection<MovimentoHasBandeira> MovimentoHasBandeira { get; set; }
        public virtual ICollection<Pagamento> Pagamento { get; set; }
    }
}
