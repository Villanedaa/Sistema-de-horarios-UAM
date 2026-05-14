using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.Entidades
{
    /// <summary>
    /// Representa un rol dentro del sistema.
    ///
    /// Los roles son utilizados para:
    /// - control de permisos,
    /// - autorización,
    /// - acceso a módulos,
    /// - policies JWT,
    /// - seguridad del sistema.
    ///
    /// Ejemplos:
    /// Administrador,
    /// Coordinador,
    /// Docente.
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Identificador único del rol.
        /// Clave primaria de la tabla Roles.
        /// </summary>
        [Key]
        public int IdRol { get; set; }

        /// <summary>
        /// Nombre del rol dentro del sistema.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el rol se encuentra activo.
        ///
        /// true  = rol habilitado
        /// false = rol deshabilitado
        /// </summary>
        public bool Activo { get; set; } = true;
    }
}