using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Pagamento
    {
        public int Idvenda { get; set; }
        public int Idmovimento { get; set; }
        public int? Credenciadora { get; set; }

        public virtual Movimento IdmovimentoNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
    }
}
