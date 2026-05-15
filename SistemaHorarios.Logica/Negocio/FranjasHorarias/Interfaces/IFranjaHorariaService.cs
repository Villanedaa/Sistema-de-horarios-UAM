using SistemaHorarios.Modelos.DTOs.FranjasHorarias;

namespace SistemaHorarios.Logica.Negocio.FranjasHorarias.Interfaces;

public interface IFranjaHorariaService
{
    Task<List<FranjaHorariaResponseDto>>
        ObtenerFranjasHorarias();

    Task<FranjaHorariaResponseDto?>
        ObtenerPorId(
            int idFranjaHoraria);

    Task CrearFranjaHoraria(
        CrearFranjaHorariaDto dto);

    Task ActualizarFranjaHoraria(
        int idFranjaHoraria,
        ActualizarFranjaHorariaDto dto);

    Task EliminarFranjaHoraria(
        int idFranjaHoraria);
}