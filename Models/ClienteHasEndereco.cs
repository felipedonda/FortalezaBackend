using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class ClienteHasEndereco
    {
        public int ClienteIdcliente { get; set; }
        public int EnderecoIdendereco { get; set; }

        public virtual Cliente ClienteIdclienteNavigation { get; set; }
        public virtual Endereco EnderecoIdenderecoNavigation { get; set; }
    }
}
