using GymSaturno.Data;

namespace GymSaturno.Repositories
{
    public interface IRepositorioGym
    {
        // CRUD de Socios
        Task<List<Socio>> GetSociosAsync();
        Task AgregarSocioAsync(Socio socio);
        Task ActualizarSocioAsync(Socio socio);
        Task EliminarSocioAsync(int id);

        // CRUD de Entrenadores y Empleados
        Task<List<Entrenador>> GetEntrenadoresAsync();
        Task AgregarEntrenadorAsync(Entrenador entrenador);
        Task EliminarEntrenadorAsync(int id);
        Task<List<Empleado>> GetEmpleadosAsync();
        Task AgregarEmpleadoAsync(Empleado empleado);
        Task EliminarEmpleadoAsync(int id);

        // Membresías, Ventas y Pagos
        Task<List<Membresia>> GetMembresiasAsync();
        Task RegistrarVentaMembresiaAsync(Membresia membresia, Pago pago);
        Task RenovarMembresiaAsync(int membresiaId, string nuevoPlan);
        Task<List<Pago>> GetPagosAsync();
        Task RegistrarAbonoPagoAsync(int pagoId, decimal montoAbono, int empleadoId);

        // Asistencias
        Task<List<Asistencia>> GetAsistenciasAsync();
        Task RegistrarAsistenciaAsync(Asistencia asistencia);
    }
}