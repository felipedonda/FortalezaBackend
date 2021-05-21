using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Preferencias
    {
        public int Idpreferencias { get; set; }
        public string NomeTaxaEntrega { get; set; }
        public int ModoTaxaEntrega { get; set; }
        public decimal TaxaEntregaPadrao { get; set; }
        public int CodigoTaxaEntrega { get; set; }
        public byte ModoEstoque { get; set; }
    }
}
