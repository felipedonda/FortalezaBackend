using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Venda
    {
        public Venda()
        {
            EstoqueHasVenda = new HashSet<EstoqueHasVenda>();
            ItemVenda = new HashSet<ItemVenda>();
            Pagamento = new HashSet<Pagamento>();
            VendaHasComanda = new HashSet<VendaHasComanda>();
        }

        public int Idvenda { get; set; }
        public int NumeroVenda { get; set; }
        public int Tipo { get; set; }
        public string Descricao { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal Desconto { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime? HoraFechamento { get; set; }
        public byte Aberta { get; set; }
        public byte Paga { get; set; }
        public decimal ValorPago { get; set; }
        public int Idresponsavel { get; set; }
        public int? Idcliente { get; set; }

        public virtual Cliente IdclienteNavigation { get; set; }
        public virtual Usuario IdresponsavelNavigation { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual ICollection<EstoqueHasVenda> EstoqueHasVenda { get; set; }
        public virtual ICollection<ItemVenda> ItemVenda { get; set; }
        public virtual ICollection<Pagamento> Pagamento { get; set; }
        public virtual ICollection<VendaHasComanda> VendaHasComanda { get; set; }
    }
}
