using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class ExamenModel
    {
        /// <summary>
        /// El identificador unico del Examen.
        /// </summary>
        [Key]
        public int ExamenId { get; set; }

        /// <summary>
        /// Representa la Categoria del examen.
        /// </summary>
        [Required(ErrorMessage = "El campo Categoría es requerido.")]
        public string Categoria { get; set; }

        /// <summary>
        /// Representa la descripcion del examen.
        /// </summary>
        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Representa el valor monetario del examen.
        /// </summary>
        [Required(ErrorMessage = "El campo Precio es requerido.")]
        //[DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        /// <summary>
        /// El identificador unico del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Representa la fecha del registro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}
