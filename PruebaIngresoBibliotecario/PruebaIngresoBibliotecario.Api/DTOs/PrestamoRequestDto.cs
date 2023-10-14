using PruebaIngresoBibliotecario.Api.Infraestructure.Enum;
using System;

namespace PruebaIngresoBibliotecario.Api.DTOs
{
    public class PrestamoRequestDto
    {
        public Guid Isbn { get; set; }
        public string IdentificacionUsuario { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }
}
