using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class InformacoesEmpresa
    {
        public int IdinformacoesEmpresa { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public int? Idendereco { get; set; }
        public int? RegimeTributario { get; set; }
        public string Cnae { get; set; }
        public string Logo { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
    }
}
