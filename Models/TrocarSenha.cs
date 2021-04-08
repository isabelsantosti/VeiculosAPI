using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class TrocarSenha
    {
        public string SenhaAntiga { get; set; }
        public string NovaSenha { get; set; }
        [Compare("NovaSenha",ErrorMessage ="A nova senha e a senha de confirmação são diferentes")]
        public string ConfirmarSenha { get; set; }

    }
}
