using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/Seguro")]
    [ApiController]
    public class SeguroController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SeguroController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Seguro.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)//API Call
        {
            var SeguroFromDb = await _db.Seguro.FirstOrDefaultAsync(u => u.SeguroId == id);
            if (SeguroFromDb == null)
            {
                return Json(new { success = false, message = "Error al Eliminar." });
            }
            _db.Seguro.Remove(SeguroFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Record Eliminar Exitosamente." });
        }
    }
}
