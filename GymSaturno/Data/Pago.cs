using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Representa el historial financiero, ventas y control de saldos pendientes
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Folio { get; set; } = string.Empty;

        [Required(ErrorMessage = "El concepto es obligatorio.")]
        [StringLength(120)]
        public string Concepto { get; set; } = string.Empty;

        // Column(TypeName = "decimal(18,2)") define la precisión exacta en SQL Server
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 50000, ErrorMessage = "El monto pagado debe estar entre $0 y $50,000.")]
        public decimal MontoPagado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 50000, ErrorMessage = "El saldo pendiente debe estar entre $0 y $50,000.")]
        public decimal MontoPendiente { get; set; }

        public DateTime FechaLimite { get; set; } = DateTime.Today.AddDays(15);
        public DateTime FechaHoraTransaccion { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }
        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el empleado que procesa el cobro.")]
        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public Empleado? Empleado { get; set; }
    }
}