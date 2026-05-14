using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.Materias;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var jwtSettings =
    builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],

                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings["Key"]!
                        )
                    )
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<PasswordService>();

// Registra los repositorios del módulo de materias.
builder.Services.AddScoped<MateriaRepository>();
builder.Services.AddScoped<PrerrequisitoRepository>();

// Registra los gestores del módulo de materias.
builder.Services.AddScoped<GestorMateria>();
builder.Services.AddScoped<GestorPrerrequisito>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();