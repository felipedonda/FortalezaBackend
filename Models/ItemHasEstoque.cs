﻿using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ItemHasEstoque
    {
        public int Iditem { get; set; }
        public int Idestoque { get; set; }

        public virtual Estoque IdestoqueNavigation { get; set; }
        public virtual Item IditemNavigation { get; set; }
    }
}
