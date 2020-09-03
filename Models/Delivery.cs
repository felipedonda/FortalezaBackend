using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Delivery
    {
        public int Iddelivery { get; set; }
        public string NomeCliente { get; set; }
        public string Endereco { get; set; }
        public byte SaiuEntrega { get; set; }
        public DateTime HoraSaiuEntrega { get; set; }
        public byte Entregue { get; set; }
    }
}
