using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Bitácora digital de ingresos diarios al gimnasio
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }

        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }
    }
}