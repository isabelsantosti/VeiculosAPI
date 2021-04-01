using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        //interface para a criação do relacionamento 1 para N
        public ICollection<Veiculo> Veiculos { get; set; }

    }
}
