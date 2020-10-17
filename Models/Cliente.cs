using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteHasMovimento = new HashSet<ClienteHasMovimento>();
            Venda = new HashSet<Venda>();
        }

        public int Idcliente { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int? Idendereco { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
        public virtual ICollection<ClienteHasMovimento> ClienteHasMovimento { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
