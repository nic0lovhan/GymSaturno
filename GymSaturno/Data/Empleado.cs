// Librería para reglas de validación como [Required] y [StringLength]
using System.ComponentModel.DataAnnotations;

namespace GymSaturno.Data
{
    // Representa la tabla 'Empleados' en SQL Server
    // guarda el id, codigo de empleado, nombre del empleado y rol
    public class Empleado
    {
        // Llave primaria autoincremental de la tabla Empleados
        [Key]
        public int Id { get; set; }

        // Número de Empleado generado automáticamente (ej. W-000001 -> 8 caracteres)
        [StringLength(8)]
        public string CodigoEmpleado { get; set; } = string.Empty;

        // Nombre del empleado (obligatorio, mínimo 3 y máximo 80 letras)
        [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreEmpleado { get; set; } = string.Empty;

        // Puesto del empleado (por defecto "Recepción")
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(40)]
        public string Rol { get; set; } = "Recepción";
    }
}