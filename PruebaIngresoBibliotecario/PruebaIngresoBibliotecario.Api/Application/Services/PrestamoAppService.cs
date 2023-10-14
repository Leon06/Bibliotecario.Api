using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Api.Application.Contracts;
using PruebaIngresoBibliotecario.Api.Domain.Contracts;
using PruebaIngresoBibliotecario.Api.DTOs;
using PruebaIngresoBibliotecario.Api.Infraestructure.Enum;
using PruebaIngresoBibliotecario.Api.Constants;

namespace PruebaIngresoBibliotecario.Api.Application.Services
{
    public class PrestamoAppService: IPrestamoAppService
    {
        #region Fields
        private readonly IPrestamoDomainService _prestamoDomainService;
        private readonly IMapper _mapper;
        private readonly ILogger<PrestamoAppService> _logger;
        #endregion

        #region Builder
        public PrestamoAppService(IPrestamoDomainService prestamoDomainService, IMapper mapper, ILogger<PrestamoAppService> logger)
        {
            _prestamoDomainService = prestamoDomainService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<ActionResult<PrestamoResponseDto>> Prestamo(PrestamoRequestDto data)
        {
            try
            {
                if (!Enum.IsDefined(typeof(TipoUsuario), data.TipoUsuario))
                {
                    return new BadRequestObjectResult(new { mensaje = ResponseMessages.InvalidUserType });
                }

                if (data.Isbn == Guid.Empty)
                {
                    return new BadRequestObjectResult(new { mensaje = ResponseMessages.InvalidIsbn });
                }

                if (string.IsNullOrWhiteSpace(data.IdentificacionUsuario) || data.IdentificacionUsuario.Length > 10)
                {
                    return new BadRequestObjectResult(new { mensaje = ResponseMessages.InvalidUserIdentification });
                }

                if (data.TipoUsuario == TipoUsuario.Invitado)
                {
                    int cantidadPrestamos = await _prestamoDomainService.CantidadDePrestamos(data.IdentificacionUsuario);
                    if (cantidadPrestamos > 0)
                    {
                        return new BadRequestObjectResult(new
                        {
                            mensaje = $"El usuario con identificacion {data.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo"
                        });
                    }
                }

                DateTime fechaDevolucion = CalcularFechaDevolucion(data.TipoUsuario, DateTime.Now);
                var prestamoGuardado = await _prestamoDomainService.GuardarDatos(data, fechaDevolucion);
                var prestamoResponDto = _mapper.Map<PrestamoResponseDto>(prestamoGuardado);

                return new OkObjectResult(prestamoResponDto);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessages.ProcessingError);
                return new BadRequestObjectResult(ResponseMessages.ProcessingError);
            }
        }
        public DateTime CalcularFechaDevolucion(TipoUsuario tipoUsuario, DateTime fechaActual)
        {
            int diasAgregar;
            switch (tipoUsuario)
            {
                case TipoUsuario.Afiliado:
                    diasAgregar = 10;
                    break;
                case TipoUsuario.EmpleadoBiblioteca:
                    diasAgregar = 8;
                    break;
                case TipoUsuario.Invitado:
                    diasAgregar = 7;
                    break;
                default:
                    throw new InvalidOperationException(ResponseMessages.InvalidUserType);

            }
            int diasTranscurridos = 0;
            while (diasTranscurridos < diasAgregar)
            {
                fechaActual = fechaActual.AddDays(1);
                if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasTranscurridos++;
                }
            }

            return fechaActual;
        }

        public async Task<ActionResult<PrestamoInfoResponseDto>> GetPrestamo(Guid idPrestamo)
        {
            try
            {
                if (idPrestamo == Guid.Empty)
                    return new BadRequestObjectResult(new { mensaje = ResponseMessages.InvalidIdentifier });

                var data = await _prestamoDomainService.GetPrestamo(idPrestamo);
                if (data != null)
                {
                    var responseDto = _mapper.Map<PrestamoInfoResponseDto>(data);
                    return new OkObjectResult(responseDto);
                }
                else
                {
                    return new NotFoundObjectResult(new { mensaje = $"El prestamo con id {idPrestamo} no existe" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessages.LoanFetchError);
                return new BadRequestObjectResult(ResponseMessages.LoanFetchError);
            }
        }
        #endregion
    }
}
