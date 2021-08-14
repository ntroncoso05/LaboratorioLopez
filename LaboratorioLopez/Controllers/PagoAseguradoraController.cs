using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/PagoAseguradora")]
    [ApiController]
    public class PagoAseguradoraController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PagoAseguradoraController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Pago.ToListAsync() });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
