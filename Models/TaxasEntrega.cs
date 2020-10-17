using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class TaxasEntrega
    {
        public int IdtaxasEntrega { get; set; }
        public int Tipo { get; set; }
        public string Bairro { get; set; }
        public decimal? Raio { get; set; }
        public decimal? Taxa { get; set; }
    }
}
