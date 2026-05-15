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

        return franjas.Select(f =>
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
        // Validación importante
        if (dto.HoraFin <= dto.HoraInicio)
        {
            throw new Exception(
                "La hora fin debe ser mayor que la hora inicio"
            );
        }

        var franjaHoraria =
            new FranjaHoraria
            {
                DiaSemana = dto.DiaSemana,

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
            throw new Exception(
                "Franja horaria no encontrada"
            );
        }

        if (dto.HoraFin <= dto.HoraInicio)
        {
            throw new Exception(
                "La hora fin debe ser mayor que la hora inicio"
            );
        }

        franja.DiaSemana =
            dto.DiaSemana;

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
            throw new Exception(
                "Franja horaria no encontrada"
            );
        }

        await _franjaHorariaRepository
            .EliminarFranjaHoraria(
                franja
            );
    }
}