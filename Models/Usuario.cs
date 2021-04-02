using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string ImageUrl { get; set; }
        //interface de coleção para a criação do relaciomaneto 1 para muitos da classe usuario para veículos
        public ICollection<Veiculo> Veiculos { get; set; }

    }
}
