using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class InformacoesEmpresa
    {
        public int IdinformacoesEmpresa { get; set; }
        public string NomeFantasia { get; set; }
        public int? Ie { get; set; }
        public int? Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public int? Idendereco { get; set; }
        public int? RegimeTributario { get; set; }
        public int? Cnae { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
    }
}
