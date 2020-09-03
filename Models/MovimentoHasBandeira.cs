using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class MovimentoHasBandeira
    {
        public int MovimentoIdmovimento { get; set; }
        public int MovimentoFormaPagamentoIdformaPagamento { get; set; }
        public int MovimentoCaixaIdcaixa { get; set; }
        public int BandeiraIdbandeira { get; set; }

        public virtual Bandeira BandeiraIdbandeiraNavigation { get; set; }
        public virtual Movimento Movimento { get; set; }
    }
}
