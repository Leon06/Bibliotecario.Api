using System;

namespace PruebaIngresoBibliotecario.Api.DTOs
{
    public class PrestamoResponseDto
    {
        public Guid Id { get; set; }
        public DateTime FechaMaximaDevolucion { get; set; }
    }
}
