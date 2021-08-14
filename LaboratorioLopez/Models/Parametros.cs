using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class Parametros
    {
        /// <summary>
        /// El identificador unico del Parametro.
        /// </summary>
        [Key]
        public int ParametroId { get; set; }

        /// <summary>
        /// Representa la descripcion del Parametro.
        /// </summary>
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Descripcion { get; set; }
    }
}
