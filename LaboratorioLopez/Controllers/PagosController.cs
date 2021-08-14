using AspNetCore.Reporting;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/Pagos")]
    [ApiController]
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PagosController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public JsonResult GetAll()
        {
            List<PagosPendienteModel> pagos = (from pago in _db.Pago.AsEnumerable()
                                                   join factura in _db.Factura.AsEnumerable() on pago.FacturaId equals factura.FacturaId
                                                   join examen in _db.Examen.AsEnumerable() on pago.ExamenId equals examen.ExamenId
                                                   join paciente in _db.Paciente.AsEnumerable() on factura.PacientId equals paciente.PacienteId
                                                   where pago.EstadoPago == false
                                                   select new PagosPendienteModel
                                                   {
                                                       PagoId = pago.PagoId,
                                                       FacturaId = pago.FacturaId,
                                                       PacienteNombre = paciente.Nombre + " " + paciente.Apellido,
                                                       Descripcion = examen.Descripcion,
                                                       PagoPaciente = pago.ExamenPrecio - pago.PagoSeguro - pago.PagoPaciente,
                                                       FechaRegistro = pago.FechaRegistro,
                                                   }).ToList();

            return Json(new { data = pagos.ToList() });
        }

        [HttpPost]
        public async Task<JsonResult> GuardarPagoToDb(List<PagosPendienteModel> valoresPagosJS)
        {
            if (valoresPagosJS.Count == 0)
                return Json(new { success = false, message = "Error al Guardar Pago. Seleccione Examanes" });

            //Obtiene el id del usuario
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Guarda record en la base datos tabla Pago.
            for (int i = 0; i < valoresPagosJS.Count; i++)
            {
                PagoModel Pagos = new PagoModel();
                //Actualiza tabla pago el estado pago.
                Pagos = await _db.Pago.FirstOrDefaultAsync(u => u.PagoId == valoresPagosJS[i].PagoId);
                Pagos.PagoPaciente += valoresPagosJS[i].PagoPaciente;
                Pagos.EstadoPago = true;
                Pagos.UsuarioId = int.Parse(claim.Value);
                _db.Pago.Update(Pagos);
            };
            await _db.SaveChangesAsync();

            List<int> pagoIdList = valoresPagosJS.Select(pago => pago.PagoId).ToList();
            TempData["reportePago"] = pagoIdList;
            
            return Json(new { success = true, message = "Pago Realizado Exitosamente." });
        }
    }
}