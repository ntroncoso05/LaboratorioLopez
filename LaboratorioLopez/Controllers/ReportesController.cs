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
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailLogic emailLogic;

        public ReportesController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, EmailLogic emailLogic)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            this.emailLogic = emailLogic;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [Route("/Reportes/Factura")]
        public IActionResult Factura() => View();

        [Route("/Reportes/Pago")]
        public IActionResult Pago() => View();

        [Route("/Reportes/ResultadoExamenes")]
        public IActionResult ResultadoExamenes() => View();

        [Route("/Reportes/PrintFactura")]
        public IActionResult PrintFactura()
        {
            int ultimoFacturaId = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
            var pagosPaciete = _db.Pago.Where(pago => pago.FacturaId == ultimoFacturaId).OrderBy(pago => pago.LineaFactura);

            int pacienteId = pagosPaciete.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            DataTable dt = new DataTable();
            dt = GetFactura();
            string mimetype = "";
            int extension = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\ReportFactura.rdlc";

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            float montoTotal = pagosPaciete.Select(pago => pago.ExamenPrecio).Sum();
            float cobertura = pagosPaciete.Select(pago => pago.PagoSeguro).Sum();
            float pagoTotal = pagosPaciete.Select(pago => pago.PagoPaciente).Sum();
            float balance = (montoTotal - cobertura - pagoTotal);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("facturaNumero", ultimoFacturaId.ToString());
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);
            parameters.Add("montoTotal", montoTotal.ToString());
            parameters.Add("cobertura", cobertura.ToString());
            parameters.Add("pagoTotal", pagoTotal.ToString());
            parameters.Add("balance", balance.ToString());
            parameters.Add("usuarioNombre", claimsIdentity.Name);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsFactura", dt);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream, "application/pdf");
        }

        private DataTable GetFactura()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LineaFactura");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Cobertura");
            dt.Columns.Add("Pago");
            dt.Columns.Add("Balance");

            int ultimoFacturaId = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
            var pagosPaciete = _db.Pago.Where(pago => pago.FacturaId == ultimoFacturaId).OrderBy(pago => pago.LineaFactura);

            int pacienteId = pagosPaciete.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = pagosPaciete.Join(_db.Examen, pago => pago.ExamenId,
                examen => examen.ExamenId, (pago, examen) => examen).ToList();
            DataRow row;
            foreach (PagoModel pago in pagosPaciete)
            {
                row = dt.NewRow();
                row["LineaFactura"] = pago.LineaFactura;
                row["Descripcion"] = examen.Where(examen => examen.ExamenId == pago.ExamenId).Select(examen => examen.Descripcion).First();
                row["Precio"] = pago.ExamenPrecio;
                row["Cobertura"] = pago.PagoSeguro;
                row["Pago"] = pago.PagoPaciente;
                row["Balance"] = pago.ExamenPrecio - pago.PagoSeguro - pago.PagoPaciente;

                dt.Rows.Add(row);
            }
            return dt;
        }

        [Route("/Reportes/ExportarExcelFactura")]
        public IActionResult ExportarExcelFactura()
        {
            var dt = new DataTable();
            dt = GetFactura();
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ReportFactura.rdlc";

            int ultimoFacturaId = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
            var pagosPaciete = _db.Pago.Where(pago => pago.FacturaId == ultimoFacturaId).OrderBy(pago => pago.LineaFactura);

            int pacienteId = pagosPaciete.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();
            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("facturaNumero", ultimoFacturaId.ToString());
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsFactura", dt);
            var result = localReport.Execute(RenderType.Excel, extension, parameters, mimetype);

            return File(result.MainStream, "application/msexcel", $"Factura {paciente.Nombre} {paciente.Apellido}.xls");
        }

        [Route("/Reportes/EnviarCorreoFactura")]
        public ViewResult EnviarCorreoFactura()
        {
            int ultimoFacturaId = _db.Factura.OrderByDescending(p => p.FacturaId).FirstOrDefault().FacturaId;
            var pagosPaciete = _db.Pago.Where(pago => pago.FacturaId == ultimoFacturaId).OrderBy(pago => pago.LineaFactura);

            int pacienteId = pagosPaciete.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = pagosPaciete.Join(_db.Examen, pago => pago.ExamenId,
                examen => examen.ExamenId, (pago, examen) => examen).ToList();

            float montoTotal = pagosPaciete.Select(pago => pago.ExamenPrecio).Sum();
            float cobertura = pagosPaciete.Select(pago => pago.PagoSeguro).Sum();
            float pagoPacienteTotal = pagosPaciete.Select(pago => pago.PagoPaciente).Sum();

            string subject = "Nueva factura de Laboratorio Lopez";
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<div>");
            body.AppendLine($"<h1>Factura #{ultimoFacturaId}</h1>");
            body.AppendLine($"<h3>Sr./Sra. {paciente.Nombre} {paciente.Apellido}:</h3>");
            body.AppendLine($"</div'>");
            body.AppendLine($"<div class='col-12 border p-2'>");
            body.AppendLine($"<table class='table table-striped table-bordered col-sm-2' style='width:100%; background-color:#C0C0C0;'>");
            body.AppendLine("<thead>");
            body.AppendLine("<tr>");
            body.Append($"<th width='{2}%'></th>");
            body.Append($"<th width='{100}%'>Descripcion</th>");
            body.Append($"<th width='{10}%'>Precio</th>");
            body.Append($"<th width='{10}%'>Cobertura</th>");
            body.Append($"<th width='{10}%'>Pago</th>");
            body.Append($"<th width='{10}%'>balance</th>");
            body.AppendLine("</tr>");
            body.AppendLine("</thead>");

            body.AppendLine("<tbody>");
            foreach (var pago in pagosPaciete)
            {
                body.AppendLine("<tr>");
                body.Append($"<th>{pago.LineaFactura}</th>");
                body.Append($"<th>{examen.Where(examen => examen.ExamenId == pago.ExamenId).Select(examen => examen.Descripcion).First()}</th>");
                body.Append($"<th>{pago.ExamenPrecio}</th>");
                body.Append($"<th>{pago.PagoSeguro}</th>");
                body.Append($"<th>{pago.PagoPaciente}</th>");
                body.Append($"<th>{pago.ExamenPrecio - pago.PagoSeguro - pago.PagoPaciente}</th>");
                body.AppendLine("</tr>");
            }
            body.AppendLine("</tbody>");
            body.AppendLine("</table>");
            body.AppendLine("<tr></tr>");
            body.AppendLine("</div>");
            body.AppendLine($"<div class='ml-auto'>");
            body.AppendLine($"<tr><strong>Monto Total: RD${montoTotal}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Cobertura Total: RD${cobertura}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Pago Inicial: RD${pagoPacienteTotal}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Balance: RD${montoTotal - cobertura - pagoPacienteTotal}.00</strong></tr>");
            body.AppendLine($"</div>");
            body.AppendLine("Saludos Laboratorio Lopez");

            var emailModel = new EmailModel(paciente.Email, subject, body.ToString(), true);
            emailLogic.EnviarEmail(emailModel);

            return View("Factura");
        }

        [Route("/Reportes/EnviarCorreoPago")]
        public ViewResult EnviarCorreoPago()
        {
            int[] pagoIdArray = TempData["reportePago"] as int[];
            TempData.Keep();
            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<PagoModel> pagoModel = _db.Pago.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);
 
            int pacienteId = pagoModel.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = pagoModel.Join(_db.Examen, pago => pago.ExamenId,
                examen => examen.ExamenId, (pago, examen) => examen).ToList();

            float montoTotal = pagoModel.Select(pago => pago.ExamenPrecio).Sum();
            float coberturaTotal = pagoModel.Select(pago => pago.PagoSeguro).Sum();
            float pagoPacienteTotal = pagoModel.Select(pago => pago.PagoPaciente).Sum();

            string subject = "Nuevo pago - Laboratorio Lopez";
            StringBuilder body = new StringBuilder();

            body.AppendLine($"<h1>Factura #{pagoModel.Select(p => p.FacturaId).FirstOrDefault()}</h1>");
            body.AppendLine($"<h3>Sr./Sra. {paciente.Nombre} {paciente.Apellido}:</h3>");
            body.AppendFormat($"<div class='col-12 border p-2'>");
            body.AppendFormat($"<table class='table table-bordered' style='width:100%; background-color:#C0C0C0;'>");
            body.AppendLine("<thead>");
            body.AppendLine("<tr>");
            body.AppendFormat($"<th style='width:2%;'></th>");
            body.AppendFormat($"<th style='width:58%;'>Descripcion</th>");
            body.AppendFormat($"<th style='width:10%;'>Precio</th>");
            body.AppendFormat($"<th style='width:10%;'>Cobertura</th>");
            body.AppendFormat($"<th style='width:10%;'>Pago</th>");
            body.AppendFormat($"<th style='width:10%;'>balance</th>");
            body.AppendLine("</tr>");
            body.AppendLine("</thead>");

            body.AppendLine("<tbody>");
            foreach (var pago in pagoModel)
            {
                //TODO - revisar al guardar monto pago y # factura.
                body.AppendLine("<tr>");
                body.Append($"<th>{pago.LineaFactura}</th>");
                body.Append($"<th>{examen.Where(examen => examen.ExamenId == pago.ExamenId).Select(examen => examen.Descripcion).First()}</th>");
                body.Append($"<th>{pago.ExamenPrecio}</th>");
                body.Append($"<th>{pago.PagoSeguro}</th>");
                body.Append($"<th>{pago.PagoPaciente}</th>");
                body.Append($"<th>{pago.ExamenPrecio - pago.PagoSeguro - pago.PagoPaciente}</th>");
                body.AppendLine("</tr>");
            }
            body.AppendLine("</tbody>");
            body.AppendLine("</table>");

            body.AppendLine("</br>");
            body.AppendLine($"<div class='col-12 border p-2 ml-auto'>");
            body.AppendLine($"<tr><strong>Monto Total: RD${montoTotal}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Cobertura Total: RD${coberturaTotal}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Pago Total: RD${pagoPacienteTotal}.00</strong></tr>");
            body.AppendLine($"<tr><strong>Balance: RD${montoTotal - coberturaTotal - pagoPacienteTotal}.00</strong></tr>");
            body.AppendLine($"</div>");
            body.AppendLine("</br>");
            body.AppendLine("Pagos de examenes completado.");
            body.AppendLine("</br>");
            body.AppendLine("Saludos Laboratorio Lopez");
            body.AppendLine("</div>");

            var emailModel = new EmailModel(paciente.Email, subject, body.ToString(), true);
            emailLogic.EnviarEmail(emailModel);
            
            return View("Pago");
        }

        [Route("/Reportes/PrintPagos")]
        public IActionResult PrintPagos()
        {
            int[] pagoIdArray = TempData["reportePago"] as int[];
            TempData.Keep();

            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<PagoModel> pagoModel = _db.Pago.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);

            int pacienteId = pagoModel.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\ReportePago.rdlc";
            
            LocalReport localReporte = new LocalReport(path);

            List<DataTable> dataTableList = new List<DataTable>();
            int extension = 1;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            float montoTotal = pagoModel.Select(pago => pago.ExamenPrecio).Sum();
            float coberturaTotal = pagoModel.Select(pago => pago.PagoSeguro).Sum();
            float pagoPacienteTotal = pagoModel.Select(pago => pago.PagoPaciente).Sum();
            float balance = (montoTotal - coberturaTotal - pagoPacienteTotal);

            DataTable dt = new DataTable();
            dt = GetPago(pagoModel);
            string mimetype = "";

            localReporte.AddDataSource("dsPago", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("facturaNumero", pagoModel.Select(p => p.FacturaId).FirstOrDefault().ToString());
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);
            parameters.Add("montoTotal", montoTotal.ToString());
            parameters.Add("cobertura", coberturaTotal.ToString());
            parameters.Add("pagoTotal", pagoPacienteTotal.ToString());
            parameters.Add("balance", balance.ToString());
            parameters.Add("usuarioNombre", claimsIdentity.Name);

            ReportResult resultado = localReporte.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(resultado.MainStream, "application/pdf");
        }

        [Route("/Reportes/ExportarExcelPago")]
        public IActionResult ExportarExcelPago()
        {
            int[] pagoIdArray = TempData["reportePago"] as int[];
            TempData.Keep();
            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<PagoModel> pagoModel = _db.Pago.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);

            int pacienteId = pagoModel.Join(_db.Factura, pago => pago.FacturaId,
                factura => factura.FacturaId, (pago, factura) => factura.PacientId).First();

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == pacienteId)
                .Select(paciente => paciente).FirstOrDefault();

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            float montoTotal = pagoModel.Select(pago => pago.ExamenPrecio).Sum();
            float coberturaTotal = pagoModel.Select(pago => pago.PagoSeguro).Sum();
            float pagoPacienteTotal = pagoModel.Select(pago => pago.PagoPaciente).Sum();
            float balance = (montoTotal - coberturaTotal - pagoPacienteTotal);

            var dt = new DataTable();
            dt = GetPago(pagoModel);
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ReportePago.rdlc";
            LocalReport localReport = new LocalReport(path);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("facturaNumero", pagoModel.Select(p => p.FacturaId).FirstOrDefault().ToString());
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);
            parameters.Add("montoTotal", montoTotal.ToString());
            parameters.Add("cobertura", coberturaTotal.ToString());
            parameters.Add("pagoTotal", pagoPacienteTotal.ToString());
            parameters.Add("balance", balance.ToString());
            parameters.Add("usuarioNombre", claimsIdentity.Name);

            localReport.AddDataSource("dsPago", dt);
            var result = localReport.Execute(RenderType.Excel, extension, parameters, mimetype);
            
            return File(result.MainStream, "application/vnd.ms-excel", $"Pago {paciente.Nombre} {paciente.Apellido}.xls");
        }

        private DataTable GetPago(IEnumerable<PagoModel> pagoModel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LineaFactura");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Precio");
            dt.Columns.Add("Cobertura");
            dt.Columns.Add("Pago");
            dt.Columns.Add("Balance");

            List<ExamenModel> examen = pagoModel.Join(_db.Examen, pago => pago.ExamenId,
                examen => examen.ExamenId, (pago, examen) => examen).ToList();

            DataRow row;
            foreach (var pago in pagoModel)
            {
                row = dt.NewRow();
                row["LineaFactura"] = pago.LineaFactura;
                row["Descripcion"] = examen.Where(examen => examen.ExamenId == pago.ExamenId).Select(examen => examen.Descripcion).First();
                row["Precio"] = pago.ExamenPrecio;
                row["Cobertura"] = pago.PagoSeguro;
                row["Pago"] = pago.PagoPaciente;
                row["Balance"] = pago.ExamenPrecio - pago.PagoSeguro - pago.PagoPaciente;

                dt.Rows.Add(row);
            }
            return dt;
        }

        [Route("/Reportes/EnviarCorreoResultadoExamenes")]
        public ViewResult EnviarCorreoResultadoExamenes()
        {
            int[] pagoIdArray = TempData["reporteResultadoExamenes"] as int[];
            TempData.Keep();
            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<EntradaExamenModel> entradaExamenModel = _db.EntradaExamen.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == entradaExamenModel.Select(paciente => paciente.PacienteId).First())
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = entradaExamenModel.Join(_db.Examen, entradaExamen => entradaExamen.ExamenId,
                examen => examen.ExamenId, (entradaExamen, examen) => examen).ToList();

            string subject = "Reporte resultado de analisis - Laboratorio Lopez";
            StringBuilder body = new StringBuilder();

            body.AppendLine($"<h1>Reporte de Analísis</h1>");
            body.AppendLine($"<h3>Sr./Sra. {paciente.Nombre} {paciente.Apellido}:</h3>");
            body.AppendLine($"<div class='col-12 border p-2'>");
            body.AppendLine($"<table class='table table-striped table-bordered' style='width:100%; background-color:#C0C0C0;'>");
            body.AppendLine("<thead>");
            body.AppendLine("<tr>");
            body.Append($"<th style='width:2%;'></th>");
            body.Append($"<th style='width:68%;'>Descripción</th>");
            body.Append($"<th style='width:10%;'>Observaciones</th>");
            body.Append($"<th style='width:10%;'>Resultado</th>");
            body.AppendLine("</tr>");
            body.AppendLine("</thead>");

            body.AppendLine("<tbody>");
            foreach (var examenPaciente in entradaExamenModel)
            {
                body.AppendLine("<tr>");
                body.Append($"<th>{examenPaciente.PagoId}</th>");
                body.Append($"<th>{examen.Where(examen => examen.ExamenId == examenPaciente.ExamenId).Select(examen => examen.Descripcion).First()}</th>");
                body.Append($"<th>{examenPaciente.ResultadoAnalisis}</th>");
                body.Append($"<th>{examenPaciente.ResultadoAnalisisParametro}</th>");
                body.AppendLine("</tr>");
            }
            body.AppendLine("</tbody>");
            body.AppendLine("</table>");
            body.AppendLine("</br>");
            body.AppendLine("Reporte de examenes completado.");
            body.AppendLine("</br>");
            body.AppendLine("Saludos Laboratorio Lopez");
            body.AppendLine("</div>");

            var emailModel = new EmailModel(paciente.Email, subject, body.ToString(), true);
            emailLogic.EnviarEmail(emailModel);

            return View("ResultadoExamenes");
        }

        [Route("/Reportes/PrintResultadoExamenes")]
        public IActionResult PrintResultadoExamenes()
        {
            int[] pagoIdArray = TempData["reporteResultadoExamenes"] as int[];
            TempData.Keep();
            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<EntradaExamenModel> entradaExamenModel = _db.EntradaExamen.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == entradaExamenModel.Select(paciente => paciente.PacienteId).First())
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = entradaExamenModel.Join(_db.Examen, entradaExamen => entradaExamen.ExamenId,
                examen => examen.ExamenId, (entradaExamen, examen) => examen).ToList();

            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\ReporteResultadoExamenes.rdlc";

            LocalReport localReporte = new LocalReport(path);

            List<DataTable> dataTableList = new List<DataTable>();
            int extension = 1;
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            DataTable dt = new DataTable();
            dt = GetResultadoExamenes(entradaExamenModel);
            string mimetype = "";

            localReporte.AddDataSource("dsResultadoExamenes", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);
            parameters.Add("usuarioNombre", claimsIdentity.Name);

            ReportResult resultado = localReporte.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(resultado.MainStream, "application/pdf");
        }

        [Route("/Reportes/ExportarExcelResultadoExamenes")]
        public IActionResult ExportarExcelResultadoExamenes()
        {
            int[] pagoIdArray = TempData["reporteResultadoExamenes"] as int[];
            TempData.Keep();
            List<int> pagoIdList = pagoIdArray.OfType<int>().ToList();

            IEnumerable<EntradaExamenModel> entradaExamenModel = _db.EntradaExamen.ToList().Join(pagoIdList, pago => pago.PagoId, id => id, (pago, id) => pago);

            PacienteModel paciente = _db.Paciente.Where(paciente => paciente.PacienteId == entradaExamenModel.Select(paciente => paciente.PacienteId).First())
                .Select(paciente => paciente).FirstOrDefault();

            List<ExamenModel> examen = entradaExamenModel.Join(_db.Examen, entradaExamen => entradaExamen.ExamenId,
                examen => examen.ExamenId, (entradaExamen, examen) => examen).ToList();

            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\ReporteResultadoExamenes.rdlc";

            LocalReport localReporte = new LocalReport(path);

            List<DataTable> dataTableList = new List<DataTable>();
            int extension = 1;
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            DataTable dt = new DataTable();
            dt = GetResultadoExamenes(entradaExamenModel);
            string mimetype = "";

            localReporte.AddDataSource("dsResultadoExamenes", dt);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("nombrePaciente", $"{paciente.Nombre} {paciente.Apellido}");
            parameters.Add("direccionPaciente", paciente.Direccion);
            parameters.Add("telefonoPaciente", paciente.Telefono);
            parameters.Add("usuarioNombre", claimsIdentity.Name);

            ReportResult resultado = localReporte.Execute(RenderType.Excel, extension, parameters, mimetype);
            return File(resultado.MainStream, "application/vnd.ms-excel", $"Resultados {paciente.Nombre} {paciente.Apellido}.xls");
        }

        private DataTable GetResultadoExamenes(IEnumerable<EntradaExamenModel> entradaExamenModel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Pago");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Observaciones");
            dt.Columns.Add("Resultado");

            List<ExamenModel> examen = entradaExamenModel.Join(_db.Examen, entradaExamen => entradaExamen.ExamenId,
                examen => examen.ExamenId, (entradaExamen, examen) => examen).ToList();

            DataRow row;
            foreach (var entrada in entradaExamenModel)
            {
                row = dt.NewRow();
                row["Pago"] = entrada.PagoId;
                row["Descripcion"] = examen.Where(examen => examen.ExamenId == entrada.ExamenId).Select(examen => examen.Descripcion).First();
                row["Observaciones"] = entrada.ResultadoAnalisis;
                row["Resultado"] = entrada.ResultadoAnalisisParametro;

                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
