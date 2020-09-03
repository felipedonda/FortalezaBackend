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
        public decimal ValorTotal { get; set; }
        public decimal? Alteracao { get; set; }
        public decimal? CustoTotal { get; set; }
        public DateTime HoraEntrada { get; set; }
        public string Observacao { get; set; }
        public byte Aberta { get; set; }
        public byte Paga { get; set; }
        public int Idresponsavel { get; set; }

        public virtual Usuario IdresponsavelNavigation { get; set; }
        public virtual ICollection<EstoqueHasVenda> EstoqueHasVenda { get; set; }
        public virtual ICollection<ItemVenda> ItemVenda { get; set; }
        public virtual ICollection<Pagamento> Pagamento { get; set; }
        public virtual ICollection<VendaHasComanda> VendaHasComanda { get; set; }
    }
}
