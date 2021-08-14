using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class PagoModel
    {
        /// <summary>
        /// El identificador unico del pago.
        /// </summary>
        [Key]
        public int PagoId { get; set; }

        /// <summary>
        /// El identificador unico de la factura.
        /// </summary>
        public int FacturaId { get; set; }

        /// <summary>
        /// El numero de las lineas de factura id.
        /// </summary>
        public int LineaFactura { get; set; }

        /// <summary>
        /// El id del examen.
        /// </summary>
        public int ExamenId { get; set; }

        /// <summary>
        /// Representa el valor del precio al facturar.
        /// </summary>
        ///[Required]
        public float ExamenPrecio { get; set; }

        /// <summary>
        /// Representa el valor del pago por paciente.
        /// </summary>
        ///[Required]
        public float PagoPaciente { get; set; }

        /// <summary>
        /// Representa el valor del pago por seguro.
        /// </summary>
        ///[Required]
        public float PagoSeguro { get; set; }

        /// <summary>
        /// El estado de la factura abierta/cerrada.
        /// </summary>
        public bool EstadoPago { get; set; }

        /// <summary>
        /// El estado resultado del analisis examen.
        /// </summary>
        public bool EstadoExamen { get; set; }

        /// <summary>
        /// El identificador unico del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Representa la fecha del registro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }
    }
}
