using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioLopez.Pages.ListaSeguro
{
    public class InicioModel : PageModel
    {
        private readonly ApplicationDbContext _db; //Using dependency injection

        public InicioModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<SeguroModel> Seguros { get; set; }

        public async Task OnGet()
        {
            Seguros = await _db.Seguro.ToListAsync();
        }
    }
}
