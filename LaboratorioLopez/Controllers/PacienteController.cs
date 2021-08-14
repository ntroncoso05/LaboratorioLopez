using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/Paciente")]
    [ApiController]
    public class PacienteController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PacienteController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Paciente.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)//API Call
        {
            var PacienteFromDb = await _db.Paciente.FirstOrDefaultAsync(u => u.PacienteId == id);
            if (PacienteFromDb == null)
            {
                return Json(new { success = false, message = "Error al Eliminar." });
            }
            _db.Paciente.Remove(PacienteFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Record Eliminado Exitosamente." });
        }
    }
}
