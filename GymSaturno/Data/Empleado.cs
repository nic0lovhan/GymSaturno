using System.ComponentModel.DataAnnotations;

namespace GymSaturno.Data
{
    // Representa la tabla 'Empleados' en SQL Server
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        // Número de Empleado generado automáticamente (ej. W-000001 -> 8 caracteres)
        [StringLength(8)]
        public string CodigoEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(40)]
        public string Rol { get; set; } = "Recepción";
    }
}