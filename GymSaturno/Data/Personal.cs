using System.ComponentModel.DataAnnotations;

namespace GymSaturno.Data
{
    // Representa la tabla 'Entrenadores' en SQL Server
    public class Entrenador
    {
        [Key]
        public int Id { get; set; }

        // Número de Entrenador generado automáticamente (ej. C-000001 -> 8 caracteres)
        [StringLength(8)]
        public string CodigoEntrenador { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del instructor es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreInstructor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona una especialidad.")]
        [StringLength(50)]
        public string Especialidad { get; set; } = "Musculación";

        [Required(ErrorMessage = "Selecciona un turno.")]
        [StringLength(30)]
        public string Turno { get; set; } = "Matutino";

        public List<Socio> Socios { get; set; } = new();
    }
}