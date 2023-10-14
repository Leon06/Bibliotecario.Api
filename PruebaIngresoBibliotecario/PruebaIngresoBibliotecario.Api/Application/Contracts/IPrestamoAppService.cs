using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Api.DTOs;

namespace PruebaIngresoBibliotecario.Api.Application.Contracts
{
    public interface IPrestamoAppService
    {
        /// <summary>
        /// Permite crear un prestamo
        /// </summary>
        /// <returns></returns>
        Task<ActionResult<PrestamoResponseDto>> Prestamo(PrestamoRequestDto data);

        /// <summary>
        /// Obtiene información el prestamo por ID.
        /// </summary>
        /// <returns></returns>
        Task<ActionResult<PrestamoInfoResponseDto>> GetPrestamo(Guid idPrestamo);
    }
}
