using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ClienteHasMovimento
    {
        public int ClienteIdcliente { get; set; }
        public int MovimentoIdmovimento { get; set; }

        public virtual Cliente ClienteIdclienteNavigation { get; set; }
        public virtual Movimento MovimentoIdmovimentoNavigation { get; set; }
    }
}
