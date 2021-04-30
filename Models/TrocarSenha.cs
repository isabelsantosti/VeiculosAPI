using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VeiculosAPI.Models
{
    public class TrocarSenha
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Senha antiga")]
        public string SenhaAntiga { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="A {0} deve conter pelo menos {2} caracteres",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name ="Nova senha")]
        public string NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirmar senha")]
        [Compare("NovaSenha",ErrorMessage ="A nova senha e a senha de confirmação são diferentes")]

        public string ConfirmarSenha { get; set; }

    }
}
