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
        public bool IsCpf
        {
            get
            {
                return Cpf.Length == 11;
            }
        }

        [NotMapped]
        public string CpfFormatted
        {
            get
            {
                if (Cpf != null)
                {
                    string formatted = "";
                    if (IsCpf)
                    {
                        formatted += Cpf.Substring(0, 3) + ".";
                        formatted += Cpf.Substring(3, 3) + ".";
                        formatted += Cpf.Substring(6, 3) + "-";
                        formatted += Cpf.Substring(9, 2);
                    }
                    else
                    {
                        formatted += Cpf.Substring(0, 2) + ".";
                        formatted += Cpf.Substring(2, 3) + ".";
                        formatted += Cpf.Substring(5, 3) + "/";
                        formatted += Cpf.Substring(8, 4) + "-";
                        formatted += Cpf.Substring(12, 2);
                    }
                    return formatted;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task AddMovimento(fortalezaitdbContext dbcontext, Movimento movimento)
        {
            ClienteHasMovimento clienteHasMovimento = new ClienteHasMovimento
            {
                Idcliente = Idcliente,
                IdmovimentoNavigation = movimento
            };

            dbcontext.ClienteHasMovimento.Add(clienteHasMovimento);
            await dbcontext.SaveChangesAsync();
        }

        [NotMapped]
        public decimal SaldoEmConta
        { 
            get
            {
                decimal creditoEmConta = 0;
                foreach(var CM in ClienteHasMovimento)
                {
                    if (CM.IdmovimentoNavigation.Tipo == 2)
                    {
                        creditoEmConta += CM.IdmovimentoNavigation.Valor;
                    }

                    if (CM.IdmovimentoNavigation.Tipo == 3)
                    {
                        creditoEmConta -= CM.IdmovimentoNavigation.Valor;
                    }
                }
                return creditoEmConta;
            }
        }
    }
}
