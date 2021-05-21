using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Fiscal
    {
        public int Iditem { get; set; }
        public int? Ncm { get; set; }
        public int? Cest { get; set; }
        public int Cfop { get; set; }
        public int Origem { get; set; }
        public int CstIcms { get; set; }
        public decimal AliquotaIcms { get; set; }

        public virtual Item IditemNavigation { get; set; }
    }
}
