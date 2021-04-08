using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class TrocarSenha
    {
        public string SenhaAntiga { get; set; }
        public string ConfirmarSenha { get; set; }
        public string NovaSenha { get; set; }
    }
}
