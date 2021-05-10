using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Motor { get; set; }
        public string Fabricante { get; set; }
        public string Quilometragem { get; set; }
        public int Ano { get; set; }
        public string Cambio { get; set; }
        public string Combustivel { get; set; }
        public string Portas { get; set; }
        public string Direcao { get; set; }
        public DateTime DataPostagem { get; set; }
        public bool isRecomendado { get; set; }
        public bool isDestaque { get; set; }
        public string Localizacao { get; set; }
        public string Condicao { get; set; }
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        //add interface para relacionamento 1 para muitos
        public ICollection<Imagem> Imagens { get; set; }
    }
}
