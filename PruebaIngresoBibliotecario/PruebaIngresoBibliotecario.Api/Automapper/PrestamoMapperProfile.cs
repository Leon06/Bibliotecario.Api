using AutoMapper;
using PruebaIngresoBibliotecario.Api.DTOs;
using PruebaIngresoBibliotecario.Api.Infraestructure;

namespace PruebaIngresoBibliotecario.Api.Automapper
{
    public class PrestamoMapperProfile : Profile
    {
        public PrestamoMapperProfile()
        {
            CreateMap<Prestamo, PrestamoResponseDto>()
                .ForMember(dest => dest.FechaMaximaDevolucion,
                            opt => opt.MapFrom(src => src.FechaDevolucion.ToString("dd/MM/yyyy"))).ReverseMap();

            CreateMap<Prestamo, PrestamoInfoResponseDto>()
                .ForMember(dest => dest.FechaMaximaDevolucion,
                            opt => opt.MapFrom(src => src.FechaDevolucion.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Isbn, opt => opt.MapFrom(src => src.Libro.Isbn))
                .ForMember(dest => dest.IdentificacionUsuario, opt => opt.MapFrom(src => src.Usuario.IdentificacionUsuario))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.Usuario.TipoUsuario));

        }
    }
}
