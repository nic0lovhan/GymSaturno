using GymSaturno.Data;
using Microsoft.EntityFrameworkCore;

namespace GymSaturno.Repositories
{
    public class RepositorioGym : IRepositorioGym
    {
        private readonly AppDBContext _db;

        public RepositorioGym(AppDBContext db)
        {
            _db = db;
        }

        // --- SOCIOS ---
        public async Task<List<Socio>> GetSociosAsync()
        {
            return await _db.Socios
                .AsNoTracking()
                .Include(s => s.Entrenador)
                .Include(s => s.Membresias)
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }

        public async Task AgregarSocioAsync(Socio socio)
        {
            _db.ChangeTracker.Clear();

            // Genera el Número de Socio consecutivo: S- + 6 dígitos (ej. S-000001)
            int ultimoId = await _db.Socios.AnyAsync() ? await _db.Socios.MaxAsync(s => s.Id) : 0;
            socio.Matricula = $"S-{(ultimoId + 1):D6}";

            _db.Socios.Add(socio);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }

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

                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }
        }

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
        public async Task<List<Entrenador>> GetEntrenadoresAsync() =>
            await _db.Entrenadores.AsNoTracking().ToListAsync();

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

        public async Task<List<Empleado>> GetEmpleadosAsync() =>
            await _db.Empleados.AsNoTracking().ToListAsync();

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
        public async Task<List<Membresia>> GetMembresiasAsync()
        {
            return await _db.Membresias
                .AsNoTracking()
                .Include(m => m.Socio)
                .Include(m => m.Empleado)
                .OrderBy(m => m.FechaVencimiento)
                .ToListAsync();
        }

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

        public async Task<List<Pago>> GetPagosAsync()
        {
            return await _db.Pagos
                .AsNoTracking()
                .Include(p => p.Socio)
                .Include(p => p.Empleado)
                .OrderByDescending(p => p.FechaHoraTransaccion)
                .ToListAsync();
        }

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
        public async Task<List<Asistencia>> GetAsistenciasAsync()
        {
            return await _db.Asistencias
                .AsNoTracking()
                .Include(a => a.Socio)
                    .ThenInclude(s => s!.Membresias)
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();
        }

        public async Task RegistrarAsistenciaAsync(Asistencia asistencia)
        {
            _db.ChangeTracker.Clear();
            _db.Asistencias.Add(asistencia);
            await _db.SaveChangesAsync();
            _db.ChangeTracker.Clear();
        }
    }
}