using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteHasEndereco = new HashSet<ClienteHasEndereco>();
        }

        public int Idcliente { get; set; }
        public string Nome { get; set; }
        public int? Cpf { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        public virtual ICollection<ClienteHasEndereco> ClienteHasEndereco { get; set; }
    }
}
