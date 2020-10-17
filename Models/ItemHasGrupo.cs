using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ItemHasGrupo
    {
        public int Iditem { get; set; }
        public int Idgrupo { get; set; }

        public virtual Grupo IdgrupoNavigation { get; set; }
        public virtual Item IditemNavigation { get; set; }
    }
}
