using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ItemHasGrupo
    {
        public int ItemIditem { get; set; }
        public int GrupoIdgrupo { get; set; }

        public virtual Grupo GrupoNavigation { get; set; }
        public virtual Item ItemNavigation { get; set; }
    }
}
