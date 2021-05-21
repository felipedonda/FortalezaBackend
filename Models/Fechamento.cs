using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Fechamento
    {
        public int Idcaixa { get; set; }
        public int Idpdv { get; set; }
        public int Idusuario { get; set; }
        public DateTime Hora { get; set; }

        public virtual Caixa IdcaixaNavigation { get; set; }
        public virtual Pdv IdpdvNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
    }
}
