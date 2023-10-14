using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Api.Infraestructure;
using PruebaIngresoBibliotecario.Api.DTOs;

namespace PruebaIngresoBibliotecario.Api.Domain.Contracts
{
    public interface IPrestamoDomainService
    {
        Task<int> CantidadDePrestamos(string identificacionUsuario);
        Task<Prestamo> GuardarDatos(PrestamoRequestDto data, DateTime fechaDevolucion);
        Task<Prestamo> GetPrestamo(Guid idPrestamo);
    }
}
