using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class PacienteModel
    {
        /// <summary>
        /// El identificador unico del paciente.
        /// </summary>
        [Key]
        public int PacienteId { get; set; }

        /// <summary>
        /// Representa el nombre del paciente.
        /// </summary>
       [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        /// <summary>
        /// Representa el apellido del paciente.
        /// </summary>
        [Required(ErrorMessage = "El campo Apellido es requerido.")]
        public string Apellido { get; set; }

        /// <summary>
        /// Representa la direccion del paciente.
        /// </summary>
        [Required(ErrorMessage = "El campo Dirección es requerido.")]
        public string Direccion { get; set; }

        /// <summary>
        /// Representa el telefono del paciente.
        /// </summary>
        [Required(ErrorMessage = "El campo Telefono es requerido.")]
        public string Telefono { get; set; }

        /// <summary>
        /// El correo electronico del paciente.
        /// </summary>
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        /// <summary>
        /// Representa la fecha nacimiento paciente.
        /// </summary>
        [Required(ErrorMessage = "El campo Fecha Nacimiento es requerido.")]
        [Display(Name ="Fecha Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        
        public int UsuarioId { get; set; }

        /// <summary>
        /// Representa la fecha del registro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}
