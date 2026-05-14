using Microsoft.EntityFrameworkCore;
using SistemaHorarios.API.Configuraciones;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.API.Middlewares;
using SistemaHorarios.API.Politicas;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<SistemaHorariosDbContext>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString(
                "DefaultConnection"
            ),
            ServerVersion.AutoDetect(
                builder.Configuration.GetConnectionString(
                    "DefaultConnection"
                )
            )
        )
);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();

// JWT
builder.Services.ConfigureJwt(
    builder.Configuration
);

// Servicios Auth
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<PasswordService>();

// Repositorios Materias
builder.Services.AddScoped<MateriaRepository>();

builder.Services.AddScoped<PrerrequisitoRepository>();

// Gestores Materias
builder.Services.AddScoped<GestorMateria>();

builder.Services.AddScoped<GestorPrerrequisito>();

// Políticas de autorización
builder.Services
    .ConfigureAuthorizationPolicies();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();