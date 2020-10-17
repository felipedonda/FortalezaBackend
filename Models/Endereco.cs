using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Endereco
    {
        public Endereco()
        {
            Cliente = new HashSet<Cliente>();
            InformacoesEmpresa = new HashSet<InformacoesEmpresa>();
            Usuario = new HashSet<Usuario>();
        }

        public int Idendereco { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<InformacoesEmpresa> InformacoesEmpresa { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
