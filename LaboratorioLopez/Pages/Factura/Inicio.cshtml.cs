using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace LaboratorioLopez.Pages.Factura
{
    public class InicioModel : PageModel
    {
        private ApplicationDbContext _db;

        public InicioModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public PacienteModel Pacientes { get; set; } = new PacienteModel();
        public FacturaModel Facturas { get; set; } = new FacturaModel();

        public async Task<IActionResult> OnGet(int? id) // ? means nullable id. Tabla informacion paciente.
        {
            Pacientes = await _db.Paciente.FirstOrDefaultAsync(u => u.PacienteId == id);

            List<SelectListItem> seguroLista = (from s in _db.Seguro.AsEnumerable()
                                                 select new SelectListItem
                                                 {
                                                     Text = s.Nombre,
                                                     Value = s.SeguroId.ToString()
                                                 }).ToList();

            //Agregar Default Item en primera Posicion DropDownList.
            seguroLista.Insert(0, new SelectListItem { Text = "[Seleccionar Seguro]", Value = "" });
            ViewData["DBSeguros"] = seguroLista;

            //Obtiene el ultimo ID de la tabla factura usando el metodo de ordenar (OrderByDescending())
            if (_db.Factura.Count()!=0)
            {
                var maxIdFactura = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
                Facturas.FacturaId = maxIdFactura + 1; 
            }
            else
            {
                Facturas.FacturaId = 1;
            }
            return Page();
        }        
    }
}
