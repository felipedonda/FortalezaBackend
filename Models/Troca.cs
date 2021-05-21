using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Troca
    {
        public Troca()
        {
            TrocaHasItemVenda = new HashSet<TrocaHasItemVenda>();
        }

        public int Idvenda { get; set; }
        public DateTime Hora { get; set; }
        public int Idusuario { get; set; }
        public int Idpdv { get; set; }

        public virtual Pdv IdpdvNavigation { get; set; }
        public virtual Usuario IdusuarioNavigation { get; set; }
        public virtual Venda IdvendaNavigation { get; set; }
        public virtual ICollection<TrocaHasItemVenda> TrocaHasItemVenda { get; set; }
    }
}
