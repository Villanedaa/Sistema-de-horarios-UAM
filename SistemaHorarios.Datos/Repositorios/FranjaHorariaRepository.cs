using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;
using System.Globalization;
using System.Text;

namespace SistemaHorarios.Datos.Repositorios;

public class FranjaHorariaRepository
{
    private readonly SistemaHorariosDbContext
        _context;

    public FranjaHorariaRepository(
        SistemaHorariosDbContext context)
    {
        _context = context;
    }

    // Obtiene todas las franjas horarias
    public async Task<List<FranjaHoraria>>
        ObtenerFranjasHorarias()
    {
        return await _context
            .FranjasHorarias
            .ToListAsync();
    }

    public async Task<bool> ExisteDuplicada(
        string diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin,
        int idFranjaExcluir = 0)
    {
        var franjas = await _context
            .FranjasHorarias
            .ToListAsync();

        string diaNormalizado = NormalizarDia(diaSemana);

        return franjas.Any(f =>
            f.IdFranjaHoraria != idFranjaExcluir &&
            NormalizarDia(f.DiaSemana) == diaNormalizado &&
            f.HoraInicio == horaInicio &&
            f.HoraFin == horaFin);
    }

    // Obtiene una franja por Id
    public async Task<FranjaHoraria?>
        ObtenerPorId(
            int idFranjaHoraria)
    {
        return await _context
            .FranjasHorarias
            .FirstOrDefaultAsync(
                f =>
                    f.IdFranjaHoraria ==
                    idFranjaHoraria
            );
    }

    // Crear franja horaria
    public async Task CrearFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        await _context
            .FranjasHorarias
            .AddAsync(franjaHoraria);

        await _context.SaveChangesAsync();
    }

    // Actualizar franja horaria
    public async Task ActualizarFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        _context
            .FranjasHorarias
            .Update(franjaHoraria);

        await _context.SaveChangesAsync();
    }

    // Eliminar franja horaria
    public async Task EliminarFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        _context
            .FranjasHorarias
            .Remove(franjaHoraria);

        await _context.SaveChangesAsync();
    }

    private static string NormalizarDia(string dia)
    {
        string texto = (dia ?? string.Empty).Trim().ToLowerInvariant();
        string normalizado = texto.Normalize(NormalizationForm.FormD);
        StringBuilder builder = new();

        foreach (char caracter in normalizado)
        {
            UnicodeCategory categoria = CharUnicodeInfo.GetUnicodeCategory(caracter);
            if (categoria != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(caracter);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
