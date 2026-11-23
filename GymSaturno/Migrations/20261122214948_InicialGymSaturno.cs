// Archivo generado automáticamente por Entity Framework al ejecutar: Add-Migration InicialGymSaturno
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSaturno.Migrations
{
    /// <inheritdoc />
    // Clase parcial que contiene las instrucciones SQL para construir o deshacer la base de datos
    public partial class InicialGymSaturno : Migration
    {
        /// <inheritdoc />
        // Método Up: se ejecuta con Update-Database para crear todas las tablas y relaciones en SQL Server
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crea la tabla Empleados con Id autoincremental (Identity 1,1) y llave primaria
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoEmpleado = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    NombreEmpleado = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            // Crea la tabla Entrenadores con su llave primaria PK_Entrenadores
            migrationBuilder.CreateTable(
                name: "Entrenadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoEntrenador = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    NombreInstructor = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenadores", x => x.Id);
                });

            // Crea la tabla Socios y la enlaza con Entrenadores; si se borra el entrenador, EntrenadorId pasa a NULL
            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricula = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntrenadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Socios_Entrenadores_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "Entrenadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Crea la tabla Asistencias enlazada al SocioId
            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asistencias_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Crea la tabla Membresias enlazada a Empleados y Socios (Restrict evita borrados en cascada conflictivos)
            migrationBuilder.CreateTable(
                name: "Membresias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membresias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Membresias_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membresias_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Crea la tabla Pagos con montos decimales (18,2) y llaves foráneas a Empleados y Socios
            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Folio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraTransaccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Crea índices en las llaves foráneas para acelerar las consultas y búsquedas en SQL Server
            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_SocioId",
                table: "Asistencias",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Membresias_EmpleadoId",
                table: "Membresias",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Membresias_SocioId",
                table: "Membresias",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_EmpleadoId",
                table: "Pagos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_SocioId",
                table: "Pagos",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_EntrenadorId",
                table: "Socios",
                column: "EntrenadorId");
        }

        /// <inheritdoc />
        // Método Down: elimina todas las tablas si en algún momento se decide revertir esta migración
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "Membresias");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "Entrenadores");
        }
    }
}