using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioLopez.Pages.ListaSeguro
{
    public class UpsertSeguroModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertSeguroModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public SeguroModel Seguros { get; set; }

        public async Task<IActionResult> OnGet(int? id) // ? means nullable id. Task<IActionResult> because we are returning to a page.
        {
            Seguros = new SeguroModel();

            if (id == null)
            {
                //Create
                return Page();
            }

            //Update
            Seguros = await _db.Seguro.FirstOrDefaultAsync(u => u.SeguroId == id);
            if (Seguros == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if (Seguros.SeguroId == 0)
                {
                    Seguros.FechaRegistro = DateTime.Now;
                    Seguros.UsuarioId = int.Parse(claim.Value);
                    _db.Seguro.Add(Seguros);
                }
                else
                {
                    Seguros.UsuarioId = int.Parse(claim.Value);
                    _db.Seguro.Update(Seguros);
                }

                await _db.SaveChangesAsync();
                return RedirectToPage("Inicio");
            }
            return RedirectToPage();
        }
    }
}
