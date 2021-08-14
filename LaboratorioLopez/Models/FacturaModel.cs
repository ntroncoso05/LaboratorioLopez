using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class FacturaModel
    {
        /// <summary>
        /// El identificador unico de la factura.
        /// </summary>
        [Key]
        public int FacturaId { get; set; }

        /// <summary>
        /// El identificador unico del paciente.
        /// </summary>        
        public int PacientId { get; set; }

        /// <summary>
        /// El identificador unico del paciente.
        /// </summary>        
        public int SeguroId { get; set; }

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
