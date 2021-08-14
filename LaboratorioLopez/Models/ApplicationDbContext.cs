using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioLopez.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<PacienteModel> Paciente { get; set; }
        public DbSet<EntradaExamenModel> EntradaExamen { get; set; }
        public DbSet<ExamenModel> Examen { get; set; }
        public DbSet<FacturaModel> Factura { get; set; }
        public DbSet<PagoModel> Pago { get; set; }
        public DbSet<Dimensiones> Dimensiones { get; set; }
        public DbSet<Parametros> Parametros { get; set; }
        public DbSet<ExamenDimensiones> ExamenDimensiones { get; set; }
        public DbSet<ExamenParametros> ExamenParametros { get; set; }
        public DbSet<SeguroModel> Seguro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ExamenDimensiones>().HasNoKey();
        }
    }
}
