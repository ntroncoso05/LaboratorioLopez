using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class PagosPendienteModel : PagoModel
    {

        /// <summary>
        /// Representa el nombre del paciente.
        /// </summary>
        public string PacienteNombre { get; set; }

        /// <summary>
        /// Representa nombre de la descripcion.
        /// </summary>
        public string Descripcion { get; set; }        
    }
}
