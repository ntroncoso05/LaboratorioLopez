using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="El campo Usuario es requerido.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password",
            ErrorMessage = "Password y confirmar password no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
