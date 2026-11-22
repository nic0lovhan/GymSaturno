using GymSaturno.Components;
using GymSaturno.Data;
using GymSaturno.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de Razor Components con interactividad en el servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el Repositorio en la Inyección de Dependencias
builder.Services.AddScoped<IRepositorioGym, RepositorioGym>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();