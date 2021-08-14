using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Usuario es requerido.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; }
    }
}
