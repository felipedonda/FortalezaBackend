using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FortalezaServer.Models
{
    public partial class Troca
    {
        [NotMapped]
        [JsonIgnore]
        public decimal ValorTotal
        {
            get
            {
                decimal value = 0;
                if (TrocaHasItemVenda.Count > 0)
                {
                    foreach (var itemTroca in TrocaHasItemVenda)
                    {
                        value += itemTroca.Quantidade * itemTroca.IditemVendaNavigation.Valor ?? default;
                    }
                }
                return value;
            }
        }
    }
}
