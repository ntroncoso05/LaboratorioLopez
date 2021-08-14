using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class EntradaExamenModel
    {
        /// <summary>
        /// El identificador unico de la entrada del examen.
        /// </summary>
        [Key]
        public int EntradaExamenId { get; set; }

        /// <summary>
        /// El identificador unico del pago examen.
        /// </summary>
        public int PagoId { get; set; }

        /// <summary>
        /// El identificador unico del Examen.
        /// </summary>
        public int ExamenId { get; set; }

        /// <summary>
        /// El identificador unico del paciente.
        /// </summary>
        public int PacienteId { get; set; }

        /// <summary>
        /// Contiene el resultado del examen.
        /// </summary>
        [Required (ErrorMessage = "El campo Resultado es requerido.")]
        public string ResultadoAnalisis { get; set; }

        /// <summary>
        /// Contiene el resultado del examen.
        /// </summary>        
        public string ResultadoAnalisisParametro { get; set; }

        /// <summary>
        /// El identificador unico del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// La fecha de registro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}
