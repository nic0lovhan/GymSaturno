// Librerías para reglas de validación y configuración de llaves foráneas
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Representa la tabla 'Socios' en SQL Server
    // guarda el id, matricula, nombre completo, telefono, correo, fecha de inscripcion, id del entrenador asignado y las listas de membresias, pagos y asistencias
    public class Socio
    {
        [Key] // Llave Primaria autoincremental interna en SQL Server
        public int Id { get; set; }

        // Número de Socio generado automáticamente (ej. S-000001 -> 8 caracteres)
        [StringLength(8)]
        public string Matricula { get; set; } = string.Empty;

        // Nombre completo del socio: obligatorio, entre 3 y 80 caracteres
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        // Teléfono: obligatorio y validado con expresión regular para aceptar exactamente 10 números
        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El teléfono debe tener exactamente 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Ingresa un teléfono válido de 10 números (ej. 3121102233).")]
        public string Telefono { get; set; } = string.Empty;

        // Correo electrónico: obligatorio y validado para que tenga formato válido con '@'
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        // Guarda automáticamente la fecha del día en que se registra el socio
        public DateTime FechaInscripcion { get; set; } = DateTime.Today;

        // Llave foránea opcional (int?) que guarda el ID del entrenador elegido en el formulario
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un entrenador de la lista.")]
        public int? EntrenadorId { get; set; } = 0;

        // Objeto de navegación para acceder a los datos completos del entrenador asignado
        [ForeignKey("EntrenadorId")]
        public Entrenador? Entrenador { get; set; }

        // Listas de navegación que conectan al socio con su historial de planes, pagos y entradas
        public List<Membresia> Membresias { get; set; } = new();
        public List<Pago> Pagos { get; set; } = new();
        public List<Asistencia> Asistencias { get; set; } = new();
    }
}