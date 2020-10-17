using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class TipoEntregador
    {
        public TipoEntregador()
        {
            Pedido = new HashSet<Pedido>();
        }

        public int IdtipoEntregador { get; set; }
        public string Nome { get; set; }
        public int? TempoMedio { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
