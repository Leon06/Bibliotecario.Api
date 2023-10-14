using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PruebaIngresoBibliotecario.Api.Application.Services;
using PruebaIngresoBibliotecario.Api.Domain.Contracts;
using PruebaIngresoBibliotecario.Api.DTOs;
using PruebaIngresoBibliotecario.Api.Infraestructure.Enum;
using PruebaIngresoBibliotecario.Api.Infraestructure;

namespace PruebaBibliotecario.Test
{
    public class PrestamoAppServiceTests
    {
        #region Fields
        //Declaración de Mocks y servicio(Arrange)
        private readonly Mock<IPrestamoDomainService> _mockPrestamoDomainService;
        private readonly Mock<ILogger<PrestamoAppService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PrestamoAppService _service;
        #endregion

        #region Builder
        public PrestamoAppServiceTests()
        {
            _mockPrestamoDomainService = new Mock<IPrestamoDomainService>();
            _mockLogger = new Mock<ILogger<PrestamoAppService>>();
            _mockMapper = new Mock<IMapper>();
            _service = new PrestamoAppService(_mockPrestamoDomainService.Object, _mockMapper.Object, _mockLogger.Object);

        }
        #endregion

        [Fact]
        public async Task GetPrestamoGuidVacio()
        {
            var result = await _service.GetPrestamo(Guid.Empty);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetPrestamoExitoso()
        {
            var prestamoFicticio = new Prestamo
            {
                Id = Guid.NewGuid(),
                Libro = new Libro
                {
                    Id = Guid.NewGuid(),
                    Isbn = Guid.NewGuid()
                },
                Usuario = new Usuario
                {
                    IdentificacionUsuario = "123456789",
                    TipoUsuario = TipoUsuario.Afiliado,
                },
                FechaDevolucion = DateTime.Now.AddDays(10)
            };

            var prestamoInfoResponse = new PrestamoInfoResponseDto
            {
                ID = prestamoFicticio.Id,
                Isbn = prestamoFicticio.Libro.Isbn,
                IdentificacionUsuario = prestamoFicticio.Usuario.IdentificacionUsuario,
                TipoUsuario = prestamoFicticio.Usuario.TipoUsuario,
                FechaMaximaDevolucion = prestamoFicticio.FechaDevolucion
            };

            _mockPrestamoDomainService.Setup(x => x.GetPrestamo(It.IsAny<Guid>()))
                                     .ReturnsAsync(prestamoFicticio);

            _mockMapper.Setup(x => x.Map<PrestamoInfoResponseDto>(It.IsAny<Prestamo>()))
                      .Returns(prestamoInfoResponse);

            // Act
            var result = await _service.GetPrestamo(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedPrestamoInfo = okResult.Value as PrestamoInfoResponseDto;
            Assert.Equal(prestamoInfoResponse, returnedPrestamoInfo);
        }

        [Fact]
        public async Task GetPrestamoNoExisteReturnsNotFound()
        {
            _mockPrestamoDomainService.Setup(x => x.GetPrestamo(It.IsAny<Guid>()))
                                     .ReturnsAsync((Prestamo)null);

            // Act
            var result = await _service.GetPrestamo(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

    }
}