using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaboratorioLopez.Pages.ListaPaciente
{
    public class UpsertPacienteModel : PageModel
    {
        private ApplicationDbContext _db;

        public UpsertPacienteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public PacienteModel Paciente { get; set; }

        public async Task<IActionResult> OnGet(int? id) // ? means nullable id. Task<IActionResult> because we are returning to a page.
        {
            Paciente = new PacienteModel();
            
            if (id == null)
            {
                //Create
                return Page();
            }

            //Update
            Paciente = await _db.Paciente.FirstOrDefaultAsync(u => u.PacienteId == id);
            if (Paciente == null)
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
                if (Paciente.PacienteId == 0)
                {
                    Paciente.FechaRegistro = DateTime.Now;
                    Paciente.UsuarioId = int.Parse(claim.Value);
                    _db.Paciente.Add(Paciente);
                }
                else
                {
                    Paciente.UsuarioId = int.Parse(claim.Value);
                    _db.Paciente.Update(Paciente);
                }

                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}

