// Librerías para validaciones y configuración de llaves foráneas
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Representa los planes contratados por los socios
    // guarda el id, plan de pago, fecha de inicio, fecha de vencimiento, id del socio y del empleado que realiza la venta
    // tambien las propiedades calculadas como estatus y proxima a vencer para evaluar si el plan sigue activo sin guardar texto estático
    public class Membresia
    {
        // Llave primaria autoincremental de la tabla Membresias
        [Key]
        public int Id { get; set; }

        // Tipo de plan contratado: Mensual, Trimestral o Anual
        [Required]
        [StringLength(50)]
        public string PlanPago { get; set; } = "Mensual";

        // Fechas de inicio y vencimiento del plan
        public DateTime FechaInicio { get; set; } = DateTime.Today;
        public DateTime FechaVencimiento { get; set; } = DateTime.Today.AddMonths(1);

        // Llave foránea del socio dueño de la membresía y su objeto de navegación
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }
        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }

        // Llave foránea del empleado que vendió el plan y su objeto de navegación
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