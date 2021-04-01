using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class Imagem
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VeiculoId { get; set; }
    }
}
