using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class Cliente
    {
        [NotMapped]
        public Endereco EnderecoPrincipal { get; set; }

        public async Task<bool> LoadEnderecoPrincipal(fortalezaitdbContext dbcontext)
        {
            await dbcontext.Entry(this).Reference(e => e.IdenderecoNavigation).LoadAsync();
            if(IdenderecoNavigation != null)
            {
                EnderecoPrincipal = IdenderecoNavigation;
                return true;
            }
            else
            {
                return false;
            }
        }

        [NotMapped]
        public decimal CreditoEmConta
        { 
            get
            {
                decimal creditoEmConta = 0;
                foreach(var CM in ClienteHasMovimento)
                {
                    if(CM.MovimentoIdmovimentoNavigation.Tipo == "C")
                    {
                        creditoEmConta += CM.MovimentoIdmovimentoNavigation.Valor;
                    }
                    else
                    {
                        creditoEmConta -= CM.MovimentoIdmovimentoNavigation.Valor;
                    }
                }
                return creditoEmConta;
            }
        }
    }
}
