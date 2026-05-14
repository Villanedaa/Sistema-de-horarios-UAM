using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Materias;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<
    SistemaHorariosDbContext
>(
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//Registrar los repositorios del modulo de materias.
builder.Services.AddScoped<MateriaRepository>();
builder.Services.AddScoped<PrerrequisitoRepository>();

//Registrar los gestores del modulo de materias.
builder.Services.AddScoped<GestorMateria>();
builder.Services.AddScoped<GestorPrerrequisito>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();