using PruebaIngresoBibliotecario.Api.Infraestructure.Enum;
using System;

namespace PruebaIngresoBibliotecario.Api.DTOs
{
    public class PrestamoInfoResponseDto
    {
        public Guid ID { get; set; }
        public Guid Isbn { get; set; }
        public string IdentificacionUsuario { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }
    }
}
