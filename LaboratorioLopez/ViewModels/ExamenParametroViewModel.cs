using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class ExamenParametroViewModel
    {
        /// <summary>
        /// El identificador unico de la dimension.
        /// </summary>
        public int ParametroId { get; set; }

        /// <summary>
        /// Representa la descripcion en unidad de medida.
        /// </summary>
        public string Descripcion { get; set; }

        public bool EsSeleccionado { get; set; }
    }
}
