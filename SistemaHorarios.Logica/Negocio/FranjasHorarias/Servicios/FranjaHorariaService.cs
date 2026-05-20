using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.FranjasHorarias.Interfaces;
using SistemaHorarios.Modelos.DTOs.FranjasHorarias;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.FranjasHorarias.Servicios;

public class FranjaHorariaService
    : IFranjaHorariaService
{
    private readonly FranjaHorariaRepository
        _franjaHorariaRepository;

    public FranjaHorariaService(
        FranjaHorariaRepository
            franjaHorariaRepository)
    {
        _franjaHorariaRepository =
            franjaHorariaRepository;
    }

    // Obtiene todas las franjas horarias
    public async Task<
        List<FranjaHorariaResponseDto>>
        ObtenerFranjasHorarias()
    {
        var franjas =
            await _franjaHorariaRepository
                .ObtenerFranjasHorarias();

        return franjas
            .OrderBy(f => ObtenerOrdenDia(f.DiaSemana))
            .ThenBy(f => f.HoraInicio)
            .Select(f =>
            new FranjaHorariaResponseDto
            {
                IdFranjaHoraria =
                    f.IdFranjaHoraria,

                DiaSemana = f.DiaSemana,

                HoraInicio = f.HoraInicio,

                HoraFin = f.HoraFin,

                Activa = f.Activa
            }).ToList();
    }

    // Obtiene una franja horaria por Id
    public async Task<
        FranjaHorariaResponseDto?>
        ObtenerPorId(
            int idFranjaHoraria)
    {
        var franja =
            await _franjaHorariaRepository
                .ObtenerPorId(
                    idFranjaHoraria);

        if (franja == null)
        {
            return null;
        }

        return new FranjaHorariaResponseDto
        {
            IdFranjaHoraria =
                franja.IdFranjaHoraria,

            DiaSemana = franja.DiaSemana,

            HoraInicio = franja.HoraInicio,

            HoraFin = franja.HoraFin,

            Activa = franja.Activa
        };
    }

    // Crea una nueva franja horaria
    public async Task CrearFranjaHoraria(
        CrearFranjaHorariaDto dto)
    {
        ValidarDatosBasicos(dto.DiaSemana, dto.HoraInicio, dto.HoraFin);

        bool existeDuplicada =
            await _franjaHorariaRepository.ExisteDuplicada(
                dto.DiaSemana,
                dto.HoraInicio,
                dto.HoraFin);

        if (existeDuplicada)
        {
            throw new InvalidOperationException(
                "Ya existe una franja horaria activa o registrada con el mismo día, hora inicio y hora fin"
            );
        }

        var franjaHoraria =
            new FranjaHoraria
            {
                DiaSemana = dto.DiaSemana.Trim(),

                HoraInicio = dto.HoraInicio,

                HoraFin = dto.HoraFin
            };

        await _franjaHorariaRepository
            .CrearFranjaHoraria(
                franjaHoraria
            );
    }

    // Actualiza una franja horaria
    public async Task ActualizarFranjaHoraria(
        int idFranjaHoraria,
        ActualizarFranjaHorariaDto dto)
    {
        var franja =
            await _franjaHorariaRepository
                .ObtenerPorId(
                    idFranjaHoraria);

        if (franja == null)
        {
            throw new InvalidOperationException(
                "Franja horaria no encontrada"
            );
        }

        ValidarDatosBasicos(dto.DiaSemana, dto.HoraInicio, dto.HoraFin);

        bool existeDuplicada =
            await _franjaHorariaRepository.ExisteDuplicada(
                dto.DiaSemana,
                dto.HoraInicio,
                dto.HoraFin,
                idFranjaHoraria);

        if (existeDuplicada)
        {
            throw new InvalidOperationException(
                "Ya existe una franja horaria con el mismo día, hora inicio y hora fin"
            );
        }

        franja.DiaSemana =
            dto.DiaSemana.Trim();

        franja.HoraInicio =
            dto.HoraInicio;

        franja.HoraFin =
            dto.HoraFin;

        franja.Activa =
            dto.Activa;

        await _franjaHorariaRepository
            .ActualizarFranjaHoraria(
                franja
            );
    }

    // Elimina una franja horaria
    public async Task EliminarFranjaHoraria(
        int idFranjaHoraria)
    {
        var franja =
            await _franjaHorariaRepository
                .ObtenerPorId(
                    idFranjaHoraria);

        if (franja == null)
        {
            throw new InvalidOperationException(
                "Franja horaria no encontrada"
            );
        }

        await _franjaHorariaRepository
            .EliminarFranjaHoraria(
                franja
            );
    }

    private static void ValidarDatosBasicos(
        string diaSemana,
        TimeSpan horaInicio,
        TimeSpan horaFin)
    {
        if (string.IsNullOrWhiteSpace(diaSemana))
        {
            throw new InvalidOperationException("El día de la semana es obligatorio");
        }

        if (horaInicio == TimeSpan.Zero && horaFin == TimeSpan.Zero)
        {
            throw new InvalidOperationException("La hora inicio y la hora fin son obligatorias");
        }

        if (horaFin <= horaInicio)
        {
            throw new InvalidOperationException("La hora fin debe ser mayor que la hora inicio");
        }
    }

    private static int ObtenerOrdenDia(string diaSemana)
    {
        string dia = (diaSemana ?? string.Empty)
            .Trim()
            .ToLowerInvariant()
            .Replace("é", "e")
            .Replace("á", "a");

        return dia switch
        {
            "lunes" => 1,
            "martes" => 2,
            "miercoles" => 3,
            "jueves" => 4,
            "viernes" => 5,
            "sabado" => 6,
            "domingo" => 7,
            _ => 99
        };
    }
}

