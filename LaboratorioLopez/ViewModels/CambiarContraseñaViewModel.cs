using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class CambiarContraseñaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva Contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva Contraseña y Confirmar Contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
