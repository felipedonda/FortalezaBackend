using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class NomeCaixa
    {
        public NomeCaixa()
        {
            Caixa = new HashSet<Caixa>();
        }

        public int IdnomeCaixa { get; set; }
        public int Nome { get; set; }

        public virtual ICollection<Caixa> Caixa { get; set; }
    }
}
