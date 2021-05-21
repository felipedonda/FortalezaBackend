using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ClienteHasMovimento
    {
        public int Idcliente { get; set; }
        public int Idmovimento { get; set; }

        public virtual Cliente IdclienteNavigation { get; set; }
        public virtual Movimento IdmovimentoNavigation { get; set; }
    }
}
