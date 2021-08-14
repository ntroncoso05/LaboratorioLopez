using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class ExamenParametros
    {
        /// <summary>
        /// El identificador unico.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// El identificador unico del examen.
        /// </summary>
        public int ExamenId { get; set; }

        /// <summary>
        /// El identificador unico  del Parametro.
        /// </summary>
        public int ParametroId { get; set; }
    }
}
