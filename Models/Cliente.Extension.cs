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
    }
}
