using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Abertura = new HashSet<Abertura>();
            Fechamento = new HashSet<Fechamento>();
            Movimento = new HashSet<Movimento>();
            Troca = new HashSet<Troca>();
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
        public virtual ICollection<Abertura> Abertura { get; set; }
        public virtual ICollection<Fechamento> Fechamento { get; set; }
        public virtual ICollection<Movimento> Movimento { get; set; }
        public virtual ICollection<Troca> Troca { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
