using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Metodo
    {
        public Metodo()
        {
            Pedido = new HashSet<Pedido>();
        }

        public int Idmetodo { get; set; }
        public byte PossuiEntregador { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
