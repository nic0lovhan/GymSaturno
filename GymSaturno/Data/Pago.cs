// Librerías para validaciones y tipos de columna en SQL Server
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSaturno.Data
{
    // Representa el historial financiero, ventas y control de saldos pendientes
    // guarda el id, folio, concepto, monto pagado, monto pendiente, fecha limite de pago, fecha y hora de la transaccion, id del socio y del empleado que procesa el cobro
    public class Pago
    {
        // Llave primaria autoincremental de la tabla Pagos
        [Key]
        public int Id { get; set; }

        // Código de recibo único (ej. F-0000000001)
        [Required]
        [StringLength(20)]
        public string Folio { get; set; } = string.Empty;

        // Descripción del cobro (ej. "Venta Membresía Mensual")
        [Required(ErrorMessage = "El concepto es obligatorio.")]
        [StringLength(120)]
        public string Concepto { get; set; } = string.Empty;

        // Column(TypeName = "decimal(18,2)") define la precisión exacta en SQL Server
        // Dinero ya pagado por el socio
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 50000, ErrorMessage = "El monto pagado debe estar entre $0 y $50,000.")]
        public decimal MontoPagado { get; set; }

        // Dinero que aún falta por liquidar (0 si ya pagó todo)
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 50000, ErrorMessage = "El saldo pendiente debe estar entre $0 y $50,000.")]
        public decimal MontoPendiente { get; set; }

        // Fecha máxima para liquidar adeudos y fecha/hora exacta del movimiento
        public DateTime FechaLimite { get; set; } = DateTime.Today.AddDays(15);
        public DateTime FechaHoraTransaccion { get; set; } = DateTime.Now;

        // Llave foránea del socio que realiza el pago y su objeto relacionado
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona un socio.")]
        public int SocioId { get; set; }
        [ForeignKey("SocioId")]
        public Socio? Socio { get; set; }

        // Llave foránea del empleado que recibió el dinero y su objeto relacionado
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el empleado que procesa el cobro.")]
        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public Empleado? Empleado { get; set; }
    }
}