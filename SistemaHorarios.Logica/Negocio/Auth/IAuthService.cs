using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaHorarios.Modelos.DTOs.Auth;

namespace SistemaHorarios.Logica.Negocio.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto dto);
    Task Registrar(RegistroUsuarioDto dto);
}
