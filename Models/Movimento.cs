using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Movimento
    {
        public Movimento()
        {
            ClienteHasMovimento = new HashSet<ClienteHasMovimento>();
            Pagamento = new HashSet<Pagamento>();
        }

        public int Idmovimento { get; set; }
        public DateTime HoraEntrada { get; set; }
        public string Descricao { get; set; }
        public int Tipo { get; set; }
        public decimal Valor { get; set; }
        public int? IdformaPagamento { get; set; }
        public int? Idcaixa { get; set; }
        public int? Idbandeira { get; set; }
        public int Idusuario { get; set; }
        public int Idpdv { get; set; }

        public virtual Bandeira IdbandeiraNavigation { get; set; }
        public virtual Caixa IdcaixaNavigation { get; set; }
        public virtual FormaPagamento IdformaPagamentoNavigation { get; set; }
        public virtual Pdv IdpdvNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
        public virtual ICollection<ClienteHasMovimento> ClienteHasMovimento { get; set; }
        public virtual ICollection<Pagamento> Pagamento { get; set; }
    }
}
