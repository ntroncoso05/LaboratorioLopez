using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/PacienteFactura")]
    [ApiController]
    public class FacturaPacienteController : Controller
    {
        private readonly ApplicationDbContext _db;
        public FacturaPacienteController(ApplicationDbContext db) => _db = db;

        public ExamenModel Examen { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Json(new { data = await _db.Examen.ToListAsync() });

        [HttpPost]
        public async Task<IActionResult> AgregarExamen(int id)
        {
            Examen = new ExamenModel();
            Examen = await _db.Examen.FirstOrDefaultAsync(u => u.ExamenId == id);
            return Json(new { dataValor = Examen, success = true, message = $"{Examen.Descripcion} Agregado a factura" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()//API Call
        {           
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Record Eliminado Exitosamente." });
        }        
    }
}
