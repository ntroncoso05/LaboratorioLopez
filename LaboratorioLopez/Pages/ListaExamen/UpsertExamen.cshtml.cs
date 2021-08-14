using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LaboratorioLopez.Models;
using LaboratorioLopez.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioLopez.Pages.ListaExamen
{
    public class UpsertExamenModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertExamenModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public EditarExamenViewModel EditarExamen { get; set; }
        
        public async Task<IActionResult> OnGet(int? id)
        {            
            ExamenModel Examen = new ExamenModel();
            EditarExamen = new EditarExamenViewModel();

            //Crear
            if (id == null) return Page();

            //Actualizar
            Examen = await _db.Examen.FirstOrDefaultAsync(u => u.ExamenId == id);
            EditarExamen = new EditarExamenViewModel()
            {
                ExamenId = Examen.ExamenId,
                Descripcion = Examen.Descripcion,
                Precio = Examen.Precio,
                Categoria = Examen.Categoria,

                Dimensiones = _db.ExamenDimensiones.Where(examenDimension => examenDimension.ExamenId == id)
                .Join(_db.Dimensiones, examenDimension => examenDimension.DimensionId, dimension => dimension.DimensionId,
                (examenDimension, dimension) => dimension.Descripcion).ToList(),

                Parametros = _db.ExamenParametros.Where(examenParametro => examenParametro.ExamenId == id)
                .Join(_db.Parametros, examenParametro => examenParametro.ParametroId, parametro => parametro.ParametroId,
                (examenParametro, parametro) => parametro.Descripcion).ToList(),
            };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if (EditarExamen.ExamenId == 0)
                {
                    EditarExamen.FechaRegistro = DateTime.Now;
                    EditarExamen.UsuarioId = int.Parse(claim.Value);
                    _db.Examen.Add(EditarExamen);
                }
                else
                {
                    EditarExamen.UsuarioId = int.Parse(claim.Value);
                    _db.Examen.Update(EditarExamen);
                }
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}
