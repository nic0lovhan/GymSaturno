// Importa los componentes visuales, el contexto de datos, el repositorio y Entity Framework
using GymSaturno.Components;
using GymSaturno.Data;
using GymSaturno.Repositories;
using Microsoft.EntityFrameworkCore;

// Crea el constructor principal de la aplicación web .NET
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Razor Components con interactividad en el servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar Entity Framework Core con SQL Server
// para que cualquier página pueda pedir datos a la base sin abrir conexiones manuales
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el Repositorio en la Inyección de Dependencias (crea una instancia por sesión de usuario)
builder.Services.AddScoped<IRepositorioGym, RepositorioGym>();

// Construye la aplicación ya configurada
var app = builder.Build();

// En producción, redirige errores críticos a /Error y activa seguridad HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Redirige rutas inexistentes a la página NotFound.razor
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// Fuerza el uso de conexión segura HTTPS
app.UseHttpsRedirection();
// Protege los formularios contra ataques de falsificación de peticiones (CSRF)
app.UseAntiforgery();

// Habilita archivos estáticos (CSS, imágenes) y mapea el componente raíz App con modo interactivo
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Arranca el servidor web de Gym Saturno
app.Run();