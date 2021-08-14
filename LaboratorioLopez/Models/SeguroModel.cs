using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class SeguroModel
    {
        /// <summary>
        /// El identificador unico del seguro.
        /// </summary>
        [Key]
        public int SeguroId { get; set; }

        /// <summary>
        /// Representa el nombre del seguro.
        /// </summary>
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        /// <summary>
        /// Representa el apellido del seguro.
        /// </summary>
        [Required(ErrorMessage = "El campo RNC es requerido.")]
        public string RNC { get; set; }

        /// <summary>
        /// Representa la direccion del seguro.
        /// </summary>
        [Required(ErrorMessage = "El campo Dirección es requerido.")]
        public string Direccion { get; set; }

        /// <summary>
        /// Representa el telefono del seguro.
        /// </summary>
        [Required(ErrorMessage = "El campo Telefono es requerido.")]
        public string Telefono { get; set; }

        /// <summary>
        /// Representa el email del seguro.
        /// </summary>
        [Required(ErrorMessage = "El campo Email es requerido.")]
        public string Email { get; set; }

        public int UsuarioId { get; set; }

        /// <summary>
        /// Representa la fecha del registro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}

