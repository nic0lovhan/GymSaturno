// Importa los modelos y Entity Framework Core para consultas SQL y métodos asíncronos
using GymSaturno.Data;
using Microsoft.EntityFrameworkCore;
// implementacion de la interfaz IRepositorioGym para interactuar con la base de datos y las operaciones CRUD (crear,leer, actualizar y borrar) de socios, entrenadores, empleados, membresías, pagos y asistencias
// contiene toda la lógica de negocio y las reglas de validación para garantizar la integridad de los datos y el correcto funcionamiento del sistema
namespace GymSaturno.Repositories
{
    // Clase que implementa todos los métodos definidos en IRepositorioGym
    public class RepositorioGym : IRepositorioGym
    {
        // Variable privada de solo lectura que guarda la conexión al contexto de base de datos
        private readonly AppDBContext _db;

        // Constructor: recibe AppDBContext por inyección de dependencias
        public RepositorioGym(AppDBContext db)
        {
            _db = db;
        }

        // --- SOCIOS ---
        // Consulta todos los socios incluyendo su entrenador y membresías (.Include), ordenados del más nuevo al más viejo
        public async Task<List<Socio>> GetSociosAsync()
        {
            return await _db.Socios
                .AsNoTracking() // Lectura rápida sin rastreo en memoria para evitar bloqueos al editar
                .Include(s => s.Entrenador)
                .Include(s => s.Membresias)
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }

        // Inserta un socio nuevo generando su código consecutivo S-000001
        public async Task AgregarSocioAsync(Socio socio)
        {
            _db.ChangeTracker.Clear(); // Limpia objetos rastreados previos en memoria

            // Genera el Número de Socio consecutivo: S- + 6 dígitos (ej. S-000001)
            int ultimoId = await _db.Socios.AnyAsync() ? await _db.Socios.MaxAsync(s => s.Id) : 0;
            socio.Matricula = $"S-{(ultimoId + 1):D6}";

            _db.Socios.Add(socio);
            await _db.SaveChangesAsync(); // Ejecuta el INSERT en SQL Server
            _db.ChangeTracker.Clear();
        }

        // Busca al socio por su Id y actualiza sus datos sin tocar su Matrícula original
        public async Task ActualizarSocioAsync(Socio socio)
        {
            _db.ChangeTracker.Clear();
            var existente = await _db.Socios.FindAsync(socio.Id);
            if (existente != null)
            {
                // Se actualizan los datos editables sin modificar el Número de Socio
                existente.NombreCompleto = socio.NombreCompleto;
                existente.Telefono = socio.Telefono;
                existente.Correo = socio.Correo;
                existente.EntrenadorId = socio.EntrenadorId;

                await _db.SaveChangesAsync(); // Ejecuta el UPDATE en SQL Server
                _db.ChangeTracker.Clear();
            }
        }

        // Elimina un socio y limpia primero sus registros dependientes (asistencias, pagos y membresías)
        public async Task EliminarSocioAsync(int id)
        {
            _db.ChangeTracker.Clear();
            var socio = await _db.Socios.FindAsync(id);
            if (socio != null)
            {
                var asistencias = await _db.Asistencias.Where(a => a.SocioId == id).ToListAsync();
                var pagos = await _db.Pagos.Where(p => p.SocioId == id).ToListAsync();
                var membresias = await _db.Membresias.Where(m => m.SocioId == id).ToListAsync();

                _db.Asistencias.RemoveRange(asistencias);
                _db.Pagos.RemoveRange(pagos);
                _db.Membresias.RemoveRange(membresias);
                _db.Socios.Remove(socio);

                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }
        }

        // --- ENTRENADORES Y EMPLEADOS ---
        // Devuelve la lista de entrenadores sin rastreo en memoria
        public async Task<List<Entrenador>> GetEntrenadoresAsync() =>
            await _db.Entrenadores.AsNoTracking().ToListAsync();

        // Guarda un entrenador nuevo asignándole su código consecutivo C-000001
        public async Task AgregarEntrenadorAsync(Entrenador entrenador)
        {
            _db.ChangeTracker.Clear();

            // Genera el Número de Entrenador consecutivo: C- + 6 dígitos (ej. C-000001)
            int ultimoId = await _db.Entrenadores.AnyAsync() ? await _db.Entrenadores.MaxAsync(e => e.Id) : 0;
            entrenador.CodigoEntrenador = $"C-{(ultimoId + 1):D6}";

            _db.Entrenadores.Add(entrenador);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }

        // Borra un entrenador y deja en null el EntrenadorId de los socios que tenía asignados
        public async Task EliminarEntrenadorAsync(int id)
        {
            _db.ChangeTracker.Clear();
            var ent = await _db.Entrenadores.FindAsync(id);
            if (ent != null)
            {
                var sociosAsignados = await _db.Socios
                    .Where(s => s.EntrenadorId == id)
                    .ToListAsync();

                foreach (var socio in sociosAsignados)
                {
                    socio.EntrenadorId = null;
                }

                _db.Entrenadores.Remove(ent);
                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }
        }

        // Devuelve la lista completa de empleados
        public async Task<List<Empleado>> GetEmpleadosAsync() =>
            await _db.Empleados.AsNoTracking().ToListAsync();

        // Registra un empleado nuevo generando su código W-000001
        public async Task AgregarEmpleadoAsync(Empleado empleado)
        {
            _db.ChangeTracker.Clear();

            // Genera el Número de Empleado consecutivo: W- + 6 dígitos (ej. W-000001)
            int ultimoId = await _db.Empleados.AnyAsync() ? await _db.Empleados.MaxAsync(e => e.Id) : 0;
            empleado.CodigoEmpleado = $"W-{(ultimoId + 1):D6}";

            _db.Empleados.Add(empleado);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }

        // Elimina un empleado únicamente si no tiene ventas ni pagos registrados en el historial
        public async Task EliminarEmpleadoAsync(int id)
        {
            _db.ChangeTracker.Clear();
            var emp = await _db.Empleados.FindAsync(id);
            if (emp != null)
            {
                bool tieneHistorial = await _db.Pagos.AnyAsync(p => p.EmpleadoId == id)
                                   || await _db.Membresias.AnyAsync(m => m.EmpleadoId == id);

                if (!tieneHistorial)
                {
                    _db.Empleados.Remove(emp);
                    await _db.SaveChangesAsync();
                    _db.ChangeTracker.Clear();
                }
            }
        }

        // --- MEMBRESÍAS Y PAGOS ---
        // Obtiene las membresías con datos de Socio y Empleado, ordenadas por fecha más próxima a vencer
        public async Task<List<Membresia>> GetMembresiasAsync()
        {
            return await _db.Membresias
                .AsNoTracking()
                .Include(m => m.Socio)
                .Include(m => m.Empleado)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();
        }

        // Guarda al mismo tiempo la membresía vendida y el recibo de pago con folio F-0000000001
        public async Task RegistrarVentaMembresiaAsync(Membresia membresia, Pago pago)
        {
            _db.ChangeTracker.Clear();

            // Genera el Folio ascendente: F- + 10 dígitos (ej. F-0000000001)
            int ultimoPagoId = await _db.Pagos.AnyAsync() ? await _db.Pagos.MaxAsync(p => p.Id) : 0;
            pago.Folio = $"F-{(ultimoPagoId + 1):D10}";

            _db.Membresias.Add(membresia);
            _db.Pagos.Add(pago);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }

        // Extiende la vigencia de una membresía sumando los meses del nuevo plan contratado
        public async Task RenovarMembresiaAsync(int membresiaId, string nuevoPlan)
        {
            _db.ChangeTracker.Clear();
            var mem = await _db.Membresias.FindAsync(membresiaId);
            if (mem != null)
            {
                int meses = nuevoPlan switch
                {
                    "Trimestral" => 3,
                    "Anual" => 12,
                    _ => 1
                };

                // Si aún está vigente suma desde su fecha de vencimiento; si ya venció, suma desde hoy
                DateTime baseFecha = mem.FechaVencimiento.Date >= DateTime.Today
                    ? mem.FechaVencimiento.Date
                    : DateTime.Today;

                mem.PlanPago = nuevoPlan;
                mem.FechaInicio = DateTime.Today;
                mem.FechaVencimiento = baseFecha.AddMonths(meses);
                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }
        }

        // Consulta todos los pagos con los datos del socio y del empleado, del más reciente al más antiguo
        public async Task<List<Pago>> GetPagosAsync()
        {
            return await _db.Pagos
                .AsNoTracking()
                .Include(p => p.Socio)
                .Include(p => p.Empleado)
                .OrderByDescending(p => p.FechaHoraTransaccion)
                .ToListAsync();
        }

        // Suma el abono al total pagado, lo resta del saldo pendiente y actualiza fecha/hora y empleado
        public async Task RegistrarAbonoPagoAsync(int pagoId, decimal montoAbono, int empleadoId)
        {
            _db.ChangeTracker.Clear();
            var pago = await _db.Pagos.FindAsync(pagoId);
            if (pago != null && montoAbono > 0 && montoAbono <= pago.MontoPendiente)
            {
                pago.MontoPagado += montoAbono;
                pago.MontoPendiente -= montoAbono;
                pago.EmpleadoId = empleadoId;
                pago.FechaHoraTransaccion = DateTime.Now;
                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }
        }

        // --- ASISTENCIAS ---
        // Trae la bitácora de asistencias junto con los datos del socio y sus membresías (.ThenInclude)
        public async Task<List<Asistencia>> GetAsistenciasAsync()
        {
            return await _db.Asistencias
                .AsNoTracking()
                .Include(a => a.Socio)
                    .ThenInclude(s => s!.Membresias)
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        // Inserta un nuevo registro de entrada en la tabla Asistencias
        public async Task RegistrarAsistenciaAsync(Asistencia asistencia)
        {
            _db.ChangeTracker.Clear();
            _db.Asistencias.Add(asistencia);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }
    }
}