using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Entregador
    {
        public Entregador()
        {
            Pedido = new HashSet<Pedido>();
        }

        public int Identregador { get; set; }
        public string Nome { get; set; }
        public byte? Ativo { get; set; }
        public byte? Disponivel { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
