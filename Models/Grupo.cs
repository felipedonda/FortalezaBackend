using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Grupo
    {
        public Grupo()
        {
            ItemHasGrupo = new HashSet<ItemHasGrupo>();
        }

        public int Idgrupo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public byte Visivel { get; set; }

        public virtual ICollection<ItemHasGrupo> ItemHasGrupo { get; set; }
    }
}
