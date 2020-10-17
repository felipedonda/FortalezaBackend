using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Caixa = new HashSet<Caixa>();
            Venda = new HashSet<Venda>();
        }

        public int Idusuario { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public int? Cpf { get; set; }
        public int? Idendereco { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
        public virtual ICollection<Caixa> Caixa { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
