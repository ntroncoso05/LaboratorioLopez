using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class Dimensiones
    {
        /// <summary>
        /// El identificador unico de la dimension.
        /// </summary>
        [Key]
        public int DimensionId { get; set; }

        /// <summary>
        /// Representa la descripcion en unidad de medida.
        /// </summary>
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Descripcion { get; set; }
    }
}
