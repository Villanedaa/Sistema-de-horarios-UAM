using SistemaHorarios.API.Configuraciones;
using SistemaHorarios.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurarSwagger();

// JWT
builder.Services.ConfigurarJWT(builder.Configuration);

// Base de datos
builder.Services.ConfigurarBaseDatos(builder.Configuration);

// Servicios y repositorios
builder.Services.RegistrarServicios();

// Políticas de autorización
builder.Services.ConfigurarPoliticas();


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5000",
                "https://localhost:5001",
                "http://localhost:5173",
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Pipeline HTTP
app.ConfigurarPipeline();

app.Run();