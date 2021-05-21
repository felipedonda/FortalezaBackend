using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Bandeira
    {
        public Bandeira()
        {
            Movimento = new HashSet<Movimento>();
        }

        public int Idbandeira { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public decimal? Taxa1 { get; set; }
        public int? Prazo1 { get; set; }
        public decimal? Taxa2 { get; set; }
        public int? Prazo2 { get; set; }

        public virtual ICollection<Movimento> Movimento { get; set; }
    }
}
