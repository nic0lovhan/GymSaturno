// Librerías para validaciones ([Range]) y llaves de base de datos ([Key], [ForeignKey])
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Bitácora digital de ingresos diarios al gimnasio
    // guarda el id, fecha y hora de ingreso y el id del socio que ingresa al gimnasio
    public class Asistencia
    {
        // Llave primaria autoincremental de la tabla Asistencias
        [Key]
        public int Id { get; set; }

        // Guarda la fecha y hora exacta en que entra el socio
        public DateTime FechaHora { get; set; } = DateTime.Now;

        // Llave foránea: ID del socio que asistió (debe ser mayor a 0)
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }

        // Objeto relacionado para poder leer los datos del socio (nombre, matrícula, etc.)
        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }
    }
}