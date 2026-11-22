using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Representa los planes contratados por los socios
    public class Membresia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanPago { get; set; } = "Mensual";

        public DateTime FechaInicio { get; set; } = DateTime.Today;
        public DateTime FechaVencimiento { get; set; } = DateTime.Today.AddMonths(1);

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }
        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el empleado que realiza la venta.")]
        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public Empleado? Empleado { get; set; }

        // ATRIBUTO DERIVADO ([NotMapped]): No crea columna en SQL; se calcula en vivo
        // comparando la fecha de vencimiento contra la fecha actual del servidor.
        [NotMapped]
        public string Estatus => FechaVencimiento.Date >= DateTime.Today ? "Activa" : "Vencida";

        // Detecta si la membresía vence hoy o dentro de los próximos 7 días
        [NotMapped]
        public bool ProximaAVencer => FechaVencimiento.Date >= DateTime.Today &&
                                      FechaVencimiento.Date <= DateTime.Today.AddDays(7);
    }
}