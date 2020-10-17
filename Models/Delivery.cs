using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Delivery
    {
        public int Iddelivery { get; set; }
        public int Idvenda { get; set; }
        public int? Idmetodo { get; set; }
        public int? IdtipoEntregador { get; set; }

        public virtual Metodo IdmetodoNavigation { get; set; }
        public virtual TipoEntregador IdtipoEntregadorNavigation { get; set; }
        public virtual Pedido IdvendaNavigation { get; set; }
    }
}
