using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pedido
    {
        public int Idvenda { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime? DataPrazo { get; set; }
        public DateTime? DataEntregue { get; set; }
        public byte? Entregue { get; set; }
        public int? Status { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime? DateRetorno { get; set; }
        public int? TempoCozinha { get; set; }
        public int? TempoEntrega { get; set; }
        public byte? Delivery { get; set; }
        public string Observacao { get; set; }
        public int? Identregador { get; set; }
        public int? Idmetodo { get; set; }
        public int? IdtipoEntregador { get; set; }

        public virtual Entregador IdentregadorNavigation { get; set; }
        public virtual Metodo IdmetodoNavigation { get; set; }
        public virtual TipoEntregador IdtipoEntregadorNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
    }
}
