using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class UsuarioHasEndereco
    {
        public int UsuarioIdusuario { get; set; }
        public int EnderecoIdendereco { get; set; }

        public virtual Endereco EnderecoIdenderecoNavigation { get; set; }
        public virtual Usuario UsuarioIdusuarioNavigation { get; set; }
    }
}
