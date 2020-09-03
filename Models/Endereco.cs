using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class Endereco
    {
        public Endereco()
        {
            ClienteHasEndereco = new HashSet<ClienteHasEndereco>();
            InformacoesEmpresaHasEndereco = new HashSet<InformacoesEmpresaHasEndereco>();
            UsuarioHasEndereco = new HashSet<UsuarioHasEndereco>();
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

        public virtual ICollection<ClienteHasEndereco> ClienteHasEndereco { get; set; }
        public virtual ICollection<InformacoesEmpresaHasEndereco> InformacoesEmpresaHasEndereco { get; set; }
        public virtual ICollection<UsuarioHasEndereco> UsuarioHasEndereco { get; set; }
    }
}
