using AspNetCore.Reporting;
using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/FacturarDb")]
    [ApiController]
    public class FacturarDbController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FacturarDbController(ApplicationDbContext db) => _db = db;

        public FacturaModel Facturas { get; set; } = new FacturaModel();

        [HttpPost]
        public IActionResult GuardarFacturaToDb(List<PagosFactura> valoresTablaJS)
        {
            if (valoresTablaJS.Count == 0)
            {
                return Json(new { success = false, message = "Error al Facturar. Seleccione Examanes y Paciente" });
            }
            //Obtiene el id del usuario
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Guarda record en la base datos tabla Factura.
            Facturas.PacientId = valoresTablaJS[0].PacienteId;
            Facturas.UsuarioId = int.Parse(claim.Value);            
            Facturas.FechaRegistro = DateTime.Now;
            _db.Factura.Add(Facturas);            
            
            //Guarda record en la base datos tabla Pago.
            for (int i = 0; i < valoresTablaJS.Count; i++)
            {
                PagoModel Pagos = new PagoModel();
                //TODO - sacar Id Factura de la vista
                //Obtiene el ultimo ID de la tabla factura usando el metodo de ordenar (OrderByDescending())
                if (_db.Factura.Count() != 0)
                {
                    var maxIdFactura = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
                    Pagos.FacturaId = maxIdFactura + 1;
                }
                else Pagos.FacturaId = 1;

                Pagos.LineaFactura = valoresTablaJS[i].LineaFactura;
                Pagos.ExamenId = valoresTablaJS[i].ExamenId;
                Pagos.ExamenPrecio = valoresTablaJS[i].ExamenPrecio;
                Pagos.PagoPaciente = valoresTablaJS[i].PagoPaciente;
                Pagos.PagoSeguro = valoresTablaJS[i].PagoSeguro;
                Pagos.UsuarioId = int.Parse(claim.Value);
                Pagos.FechaRegistro = DateTime.Now;
                Pagos.EstadoExamen = false;

                if(valoresTablaJS[i].Balance == 0) Pagos.EstadoPago = true;

                _db.Pago.Add(Pagos);
            }
            _db.SaveChanges();

            return Json(new { success = true, message = "Cliente Facturado Exitosamente." });
        }
    }

    public class PagosFactura
    {
        /// <summary>
        /// Numero linea de la factura.
        /// </summary>
        public int LineaFactura { get; set; }

        /// <summary>
        /// El id del paciente.
        /// </summary>
        public int PacienteId { get; set; }

        /// <summary>
        /// El id del examen.
        /// </summary>
        public int ExamenId { get; set; }

        /// <summary>
        /// Representa el valor del precio al facturar.
        /// </summary>
        public float ExamenPrecio { get; set; }

        /// <summary>
        /// Representa el valor del pago por paciente.
        /// </summary>
        public float PagoPaciente { get; set; }

        /// <summary>
        /// Representa el valor del pago por seguro.
        /// </summary>
        public float PagoSeguro { get; set; }

        /// <summary>
        /// Representa el valor del balance
        /// </summary>
        //public int SeguroId { get; set; }
        public float Balance { get; set; }
    }
}
