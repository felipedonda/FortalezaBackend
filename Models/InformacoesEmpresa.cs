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
        public int? CstPis { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public byte? IndiceRateioIssqn { get; set; }
        public int? Csosn { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public virtual Endereco IdenderecoNavigation { get; set; }
    }
}
