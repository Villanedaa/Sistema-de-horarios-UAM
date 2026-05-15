using SistemaHorarios.API.Configuraciones;
using SistemaHorarios.API.Extensions;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;

// Punto de entrada principal de la aplicación.
var builder = WebApplication.CreateBuilder(args);


// =========================
// Controllers
// =========================
//usuarios 

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// Registra los controladores de la API.
builder.Services.AddControllers();


// =========================
// Swagger
// =========================

// Habilita exploración de endpoints.
builder.Services.AddEndpointsApiExplorer();

// Configura Swagger/OpenAPI.
builder.Services.ConfigurarSwagger();


// =========================
// JWT
// =========================

// Configura autenticación JWT.
builder.Services.ConfigurarJWT(
    builder.Configuration
);


// =========================
// Base de Datos
// =========================

// Configura Entity Framework Core
// y conexión MySQL.
builder.Services.ConfigurarBaseDatos(
    builder.Configuration
);


// =========================
// Servicios
// =========================

// Registra servicios, gestores
// y repositorios del sistema.
builder.Services.RegistrarServicios();


// =========================
// Políticas
// =========================

// Configura políticas de autorización.
builder.Services.ConfigurarPoliticas();


var app = builder.Build();


// =========================
// Pipeline HTTP
// =========================

// Configura middleware,
// autenticación,
// autorización
// y endpoints.
app.ConfigurarPipeline();


// Inicia la aplicación.
app.Run();