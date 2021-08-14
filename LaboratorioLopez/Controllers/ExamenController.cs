using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/Examen")]
    [ApiController]
    public class ExamenController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ExamenController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Json(new { data = await _db.Examen.ToListAsync() });

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)//API Call
        {
            var ExamenFromDb = await _db.Examen.FirstOrDefaultAsync(u => u.ExamenId == id);
            if (ExamenFromDb == null)
            {
                return Json(new { success = false, message = "Error al Eliminar." });
            }
            _db.Examen.Remove(ExamenFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Record Eliminar Exitosamente." });
            
        }
    }
}
