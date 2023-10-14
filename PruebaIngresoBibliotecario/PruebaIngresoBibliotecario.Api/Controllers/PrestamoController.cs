using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Api.Application.Contracts;
using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Api.DTOs;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        #region Fields
        private readonly IPrestamoAppService _prestamoAppService;
        #endregion

        #region Builder
        public PrestamoController(IPrestamoAppService prestamoAppService)
        {
            _prestamoAppService = prestamoAppService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Permite crear un prestamo
        /// </summary>
        /// <param name="data">Información para pedir un prestamo.</param>        
        /// <returns>Información del préstamo creado.</returns>       
        [HttpPost]
        public async Task<ActionResult<PrestamoResponseDto>> Prestamo(PrestamoRequestDto data)
        {
            return await _prestamoAppService.Prestamo(data);
        }
        /// <summary>
        /// Obtiene información el prestamo por ID.
        /// </summary> 
        /// <param name="id-prestamo">ID del préstamo.</param>   
        /// <returns>Información del préstamo.</returns>       
        [HttpGet("{idPrestamo}")]
        public async Task<ActionResult<PrestamoInfoResponseDto>> Prestamo(Guid idPrestamo)
        {
            return await _prestamoAppService.GetPrestamo(idPrestamo);
        }

        #endregion 


    }
}
