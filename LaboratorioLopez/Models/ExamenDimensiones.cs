using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{

    public class ExamenDimensiones
    {
        /// <summary>
        /// El identificador unico del ExamenDimensiones.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// El identificador unico del examen.
        /// </summary>
        public int ExamenId { get; set; }

        /// <summary>
        /// El identificador unico de la dimension.
        /// </summary>
        public int DimensionId { get; set; }
    }
}
