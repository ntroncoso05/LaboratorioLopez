using LaboratorioLopez.Models;
using LaboratorioLopez.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    public class AdministrarExamenesController : Controller
    {
        private readonly ApplicationDbContext db;

        public AdministrarExamenesController(ApplicationDbContext db) => this.db = db;

        [Route("/AdministrarExamenes/AdministrarDimensiones")] [HttpGet]
        public async Task<ViewResult> AdministrarDimensiones(string examenId)
        {
            var dimesionesEnExamenId = db.Dimensiones.Join(db.ExamenDimensiones.Where(examenDimension => examenDimension.ExamenId == int.Parse(examenId)),
                           dimension => dimension.DimensionId, examenDimension => examenDimension.DimensionId, (dimension, examenDimension) =>
                            examenDimension);
            ViewBag.examenId = examenId;
            ViewBag.dimensionId = dimesionesEnExamenId.Select(id => id.DimensionId).FirstOrDefault();
            var model = new ExamenDimensionViewModel();
            model.dimensiones = new List<ExamenDimensionViewModel>();
            
            if (await db.Dimensiones.CountAsync() != 0)
            {
                foreach (var dimension in db.Dimensiones)
                {
                    ExamenDimensionViewModel dimensionViewModel = new ExamenDimensionViewModel
                    {
                        DimensionId = dimension.DimensionId,
                        Descripcion = dimension.Descripcion,
                    };
                    

                    dimensionViewModel.EsSeleccionado = await dimesionesEnExamenId.Where(p => p.DimensionId == dimension.DimensionId).FirstOrDefaultAsync() != null;
                    model.dimensiones.Add(dimensionViewModel);
                }
            }
            return View(model);
        }

        [Route("/AdministrarExamenes/AdministrarDimensiones")] [HttpPost]
        public async Task<IActionResult> AdministrarDimensiones(ExamenDimensionViewModel examenDimensiones, string examenId)
        {
            var dimensiones = db.ExamenDimensiones.Where(dimension => dimension.ExamenId == int.Parse(examenId));
            db.ExamenDimensiones.RemoveRange(dimensiones);

            //foreach (var dimension in examenDimensionesList)
            //{
                if (examenDimensiones.DimensionId != 0)
                   await db.ExamenDimensiones.AddAsync(new ExamenDimensiones { DimensionId = examenDimensiones.DimensionId, ExamenId = int.Parse(examenId), });                    
            //}
            await db.SaveChangesAsync();
            
            return RedirectToPage("/ListaExamen/UpsertExamen", new { Id = examenId });
        }

        [Route("/AdministrarExamenes/AdministrarParametros")] [HttpGet]
        public async Task<ViewResult> AdministrarParametros(string examenId)
        {
            ViewBag.examenId = examenId;

            var model = new List<ExamenParametroViewModel>();
            if (await db.Parametros.CountAsync() != 0)
            {
                foreach (var parametro in db.Parametros)
                {
                    ExamenParametroViewModel examenParametroViewModel = new ExamenParametroViewModel
                    {
                        ParametroId = parametro.ParametroId,
                        Descripcion = parametro.Descripcion,
                    };
                    var parametrosEnExamenId = db.Parametros.Join(db.ExamenParametros.Where(examenParametro => examenParametro.ExamenId == int.Parse(examenId)),
                        parametro => parametro.ParametroId, examenParametro => examenParametro.ParametroId, (parametro, examenParametro) => examenParametro);

                    examenParametroViewModel.EsSeleccionado = await parametrosEnExamenId.Where(p => p.ParametroId == parametro.ParametroId).FirstOrDefaultAsync() != null;

                    model.Add(examenParametroViewModel);
                }
            }
            return View(model);
        }

        [Route("/AdministrarExamenes/AdministrarParametros")] [HttpPost]
        public async Task<ActionResult> AdministrarParametros(List<ExamenParametroViewModel> examenParametrosList, string examenId)
        {
            var parametros = db.ExamenParametros.Where(parametro => parametro.ExamenId == int.Parse(examenId));
            db.ExamenParametros.RemoveRange(parametros);

            foreach (var parametro in examenParametrosList)
            {
                if (parametro.EsSeleccionado)
                    await db.ExamenParametros.AddAsync(new ExamenParametros { ParametroId = parametro.ParametroId, ExamenId = int.Parse(examenId), });
            }
            await db.SaveChangesAsync();

            return RedirectToPage("/ListaExamen/UpsertExamen", new { Id = examenId });
        }

        [Route("/AdministrarExamenes/CrearDimension")]
        public async Task<JsonResult> CrearDimension(string descripcion)
        {
            if (descripcion == null)
                return Json(new { success = false, message = "Error al Crear Dimensión." });

            Dimensiones dimensiones = new Dimensiones() { Descripcion = descripcion, };
            await db.Dimensiones.AddAsync(dimensiones);            
            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Dimensión Creada Exitosamente." });
        }

        [Route("/AdministrarExamenes/CrearParametro")]
        public async Task<JsonResult> CrearParametro(string descripcion)
        {
            if (descripcion == "" || descripcion == null)
                return Json(new { success = false, message = "Error al Crear Parametro." });

            Parametros parametro = new Parametros() { Descripcion = descripcion, };
            await db.Parametros.AddAsync(parametro);
            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Parametro Creado Exitosamente." });
        }
    }
}
