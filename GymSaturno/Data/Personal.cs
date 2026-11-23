// Librería que contiene etiquetas de validación como [Required] y [StringLength]
using System.ComponentModel.DataAnnotations;

// Agrupa esta clase dentro de la carpeta Data del proyecto
namespace GymSaturno.Data
{
    // Representa la tabla 'Entrenadores' en SQL Server
    // guarda el id, codigo de entrenador, nombre del instructor, especialidad, turno y la lista de socios asignados
    public class Entrenador
    {
        // Llave primaria autoincremental de la tabla Entrenadores
        [Key]
        public int Id { get; set; }

        // Número de Entrenador generado automáticamente (ej. C-000001 -> 8 caracteres)
        [StringLength(8)]
        public string CodigoEntrenador { get; set; } = string.Empty;

        // Nombre del instructor: campo obligatorio de entre 3 y 80 caracteres
        [Required(ErrorMessage = "El nombre del instructor es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreInstructor { get; set; } = string.Empty;

        // Área de entrenamiento (arranca con "Musculación" por defecto)
        [Required(ErrorMessage = "Selecciona una especialidad.")]
        [StringLength(50)]
        public string Especialidad { get; set; } = "Musculación";

        // Horario laboral del entrenador (arranca con "Matutino" por defecto)
        [Required(ErrorMessage = "Selecciona un turno.")]
        [StringLength(30)]
        public string Turno { get; set; } = "Matutino";

        // Relación 1 a muchos: lista de socios que tienen asignado a este entrenador
        public List<Socio> Socios { get; set; } = new();
    }
}