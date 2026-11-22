using Microsoft.EntityFrameworkCore;

namespace GymSaturno.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Socio> Socios { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evita conflictos de múltiples rutas de borrado en cascada en SQL Server
            modelBuilder.Entity<Membresia>()
                .HasOne(m => m.Socio)
                .WithMany(s => s.Membresias)
                .HasForeignKey(m => m.SocioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Socio)
                .WithMany(s => s.Pagos)
                .HasForeignKey(p => p.SocioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Permite eliminar un entrenador dejando en NULL la asignación de sus socios
            modelBuilder.Entity<Socio>()
                .HasOne(s => s.Entrenador)
                .WithMany(e => e.Socios)
                .HasForeignKey(s => s.EntrenadorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}