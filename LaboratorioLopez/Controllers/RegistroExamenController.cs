using LaboratorioLopez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{
    [Route("api/RegistroExamen")]
    [ApiController]
    public class RegistroExamenController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailLogic emailLogic;

        public RegistroExamenController(ApplicationDbContext db, EmailLogic emailLogic)
        {
            _db = db;
            this.emailLogic = emailLogic;
        }

        [HttpGet]
        public IActionResult CargarListaExamen()
        {
            List<RegistroExamen> registroExamen = (from rExamen in _db.Pago.AsEnumerable()
                                                  join fPaciente in _db.Factura.AsEnumerable() on rExamen.FacturaId equals fPaciente.FacturaId
                                                  join examenes in _db.Examen.AsEnumerable() on rExamen.ExamenId equals examenes.ExamenId
                                                  join pacientes in _db.Paciente.AsEnumerable() on fPaciente.PacientId equals pacientes.PacienteId
                                                  where rExamen.EstadoExamen == false
                                                  select new RegistroExamen
                                                  {
                                                      PagoId=rExamen.PagoId,
                                                      PacienteId=pacientes.PacienteId,
                                                      PacienteNombre = pacientes.Nombre,
                                                      PacienteApellido = pacientes.Apellido,
                                                      ExamenId=rExamen.ExamenId,
                                                      ExamenDescripcion=examenes.Descripcion,
                                                      FechaRegistro = rExamen.FechaRegistro,

                                                      Dimensiones =string.Join(" - ", _db.ExamenDimensiones.Where(examen => examen.ExamenId == 
                                                      rExamen.ExamenId).Join(_db.Dimensiones, examenDimensiones => 
                                                      examenDimensiones.DimensionId, dimension => dimension.DimensionId,
                                                      (examenDimension, dimension)=> dimension.Descripcion).ToList()),

                                                      Parametros = _db.ExamenParametros.Where(examen => examen.ExamenId == rExamen.ExamenId)
                                                      .Join(_db.Parametros, examenParametro => examenParametro.ParametroId, parametro => parametro.ParametroId, 
                                                      (examenParametro, parametro) => parametro).OrderBy(p => p.Descripcion).ToList(),

                                                  }).ToList();

            return Json(new { data= registroExamen.ToList()});
        }

        [HttpPost]
        public async Task<IActionResult> GuardarResultadoExamenToDb(List<EntradaExamenModel> valoresTablaJS)
        {
            
            //Verifica si la si la variable tipo List<EntradaExamenModel> esta vacia.
            if (valoresTablaJS.Count == 0)
                return Json(new { success = false, message = "Error: Debe completar/seleccionar el campo resultado del examen" });

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            PagoModel EstadoExamen = new PagoModel();
            EntradaExamenModel Entradas = new EntradaExamenModel();

            //Guarda record en la base datos tabla EntradaExamen.
            for (int i = 0; i < valoresTablaJS.Count; i++)
            {
                Entradas = valoresTablaJS[i];
                Entradas.PacienteId = await _db.Pago.Where(pago => pago.PagoId == valoresTablaJS[i].PagoId).Join(_db.Factura, pago => pago.FacturaId, 
                    factura => factura.FacturaId, (pago, factura) => factura.PacientId).FirstAsync();
                Entradas.ExamenId = await _db.Pago.Where(pago => pago.PagoId == valoresTablaJS[i].PagoId).Select(examen => examen.ExamenId).FirstAsync();
                Entradas.FechaRegistro = DateTime.Now;
                Entradas.UsuarioId = int.Parse(claim.Value);

                //Actualiza tabla pago el examen estado de la entrad del examen.
                EstadoExamen = _db.Pago.FirstOrDefault(u => u.PagoId == valoresTablaJS[i].PagoId);
                EstadoExamen.EstadoExamen = true;
                _db.Pago.Update(EstadoExamen);

                //Agrega una nueva entrada al registro del resultado del examen.
                _db.EntradaExamen.Add(Entradas);
            }
            //Guarda los cambios en la base de datos.
            _db.SaveChanges();

            List<int> pagoIdList = valoresTablaJS.Select(pago => pago.PagoId).ToList();
            //var EntradaExamenList = _db.EntradaExamen.ToList().Join(pagoIdList, entrada => entrada.PagoId, id => id, (entrada, id) => entrada)
            //    .GroupBy(entrada => entrada.PacienteId).ToList();

            TempData["reporteResultadoExamenes"] = pagoIdList;

            return Json(new { success = true, message = "Resultado Examen Guardado Exitosamente." });
        }        
    }
}

//TODO ---->
public class RegistroExamen
{
    /// <summary>
    /// El id del examen.
    /// </summary>
    public int PagoId { get; set; }

    /// <summary>
    /// El id del paciente.
    /// </summary>
    public int PacienteId { get; set; }

    /// <summary>
    /// El id del examen.
    /// </summary>
    public int ExamenId { get; set; }

    /// <summary>
    /// Representa nombre de la descripcion.
    /// </summary>
    public string ExamenDescripcion { get; set; }

    /// <summary>
    /// Representa el nombre del paciente.
    /// </summary>
    public string PacienteNombre { get; set; }

    /// <summary>
    /// Representa el apellido del paciente.
    /// </summary>
    public string PacienteApellido { get; set; }

    /// <summary>
    /// La fecha de facturacion.
    /// </summary>
    public DateTime FechaRegistro { get; set; }

    public string Dimensiones { get; set; }

    public List<Parametros> Parametros { get; set; }
}