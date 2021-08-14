using LaboratorioLopez.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.ViewModels
{
    public class EditarExamenViewModel : ExamenModel
    {
        public List<string> Dimensiones { get; set; }
        public List<string> Parametros { get; set; }
    }
}
