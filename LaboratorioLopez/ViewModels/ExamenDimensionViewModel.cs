using LaboratorioLopez.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class ExamenDimensionViewModel
    {
        /// <summary>
        /// El identificador unico de la dimension.
        /// </summary>
        public int DimensionId { get; set; }

        ///// <summary>
        ///// Representa la descripcion en unidad de medida.
        ///// </summary>
        public string Descripcion { get; set; }

        public bool EsSeleccionado { get; set; }
        public List<ExamenDimensionViewModel> dimensiones { get; set; }
    }
}
