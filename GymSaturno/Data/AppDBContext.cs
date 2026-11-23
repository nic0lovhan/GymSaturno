// Importa Entity Framework Core para conectar las clases con SQL Server
using Microsoft.EntityFrameworkCore;

namespace GymSaturno.Data
{
    // Clase puente entre C# y la base de datos; representa toda la BD
    public class AppDBContext : DbContext
    {
        // Constructor que recibe la configuración y cadena de conexión desde Program.cs
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        // Cada DbSet representa una tabla real en SQL Server
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        // Método donde se configuran las relaciones y reglas al borrar registros
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evita conflictos de múltiples rutas de borrado en cascada en SQL Server
            modelBuilder.Entity<Membresia>()
                .HasOne(m => m.Socio)
                .WithMany(s => s.Membresias)
                .HasForeignKey(m => m.SocioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Impide borrar en cascada los pagos si se intenta eliminar un socio
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Socio)
                .WithMany(s => s.Pagos)
                .HasForeignKey(p => p.SocioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Permite eliminar un entrenador dejando en NULL la asignación de sus socios
            // para que al eliminar un entrenador no se borren sus socios

            modelBuilder.Entity<Socio>()
                .HasOne(s => s.Entrenador)
                .WithMany(e => e.Socios)
                .HasForeignKey(s => s.EntrenadorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
// Nota: Para crear la base de datos y las tablas en SQL Server, se utiliza Entity Framework Core con migraciones.
// Entity Framework leyó las clases de Data/ y crea la base de datos GymSaturno DB con sus llaves primarias, llaves foráneas y restricciones en SQL Server
//en la consola de administrador de paquetes Nuget:
// PM> Add-Migration InicialGymSaturno
// PM> Update-Database