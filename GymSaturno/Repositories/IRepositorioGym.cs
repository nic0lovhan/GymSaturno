// Importa las clases modelo (Socio, Pago, Membresia, etc.)
using GymSaturno.Data;
// interfaz para el repositorio de datos del gimnasio, define los métodos que se implementarán en RepositorioGym

namespace GymSaturno.Repositories
{
    // Interfaz (contrato): lista todos los métodos asíncronos (Task) disponibles para las páginas Razor
    public interface IRepositorioGym
    {
        // CRUD de Socios: consultar, agregar, editar y eliminar
        Task<List<Socio>> GetSociosAsync();
        Task AgregarSocioAsync(Socio socio);
        Task ActualizarSocioAsync(Socio socio);
        Task EliminarSocioAsync(int id);

        // CRUD de Entrenadores y Empleados: consultar, agregar y eliminar
        Task<List<Entrenador>> GetEntrenadoresAsync();
        Task AgregarEntrenadorAsync(Entrenador entrenador);
        Task EliminarEntrenadorAsync(int id);
        Task<List<Empleado>> GetEmpleadosAsync();
        Task AgregarEmpleadoAsync(Empleado empleado);
        Task EliminarEmpleadoAsync(int id);

        // Membresías, Ventas y Pagos: consultar planes, registrar ventas, renovar y abonar saldos
        Task<List<Membresia>> GetMembresiasAsync();
        Task RegistrarVentaMembresiaAsync(Membresia membresia, Pago pago);
        Task RenovarMembresiaAsync(int membresiaId, string nuevoPlan);
        Task<List<Pago>> GetPagosAsync();
        Task RegistrarAbonoPagoAsync(int pagoId, decimal montoAbono, int empleadoId);

        // Asistencias: consultar bitácora y registrar entradas
        Task<List<Asistencia>> GetAsistenciasAsync();
        Task RegistrarAsistenciaAsync(Asistencia asistencia);
    }
}