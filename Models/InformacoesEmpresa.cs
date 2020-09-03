using System;
using System.Collections.Generic;

namespace FortalezaServer.Models
{
    public partial class InformacoesEmpresa
    {
        public InformacoesEmpresa()
        {
            InformacoesEmpresaHasEndereco = new HashSet<InformacoesEmpresaHasEndereco>();
        }

        public int IdinformacoesEmpresa { get; set; }
        public string NomeFantasia { get; set; }
        public int? Cnpj { get; set; }
        public string RazaoSocial { get; set; }

        public virtual ICollection<InformacoesEmpresaHasEndereco> InformacoesEmpresaHasEndereco { get; set; }
    }
}
