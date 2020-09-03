using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pagamento
    {
        public int VendaIdvenda { get; set; }
        public int MovimentoIdmovimento { get; set; }
        public int MovimentoFormaPagamentoIdformaPagamento { get; set; }
        public int MovimentoCaixaIdcaixa { get; set; }

        public virtual Movimento Movimento { get; set; }
        public virtual Venda VendaIdvendaNavigation { get; set; }
    }
}
