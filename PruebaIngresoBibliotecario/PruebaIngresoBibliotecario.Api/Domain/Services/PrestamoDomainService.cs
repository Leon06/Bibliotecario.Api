using System.Threading.Tasks;
using System;
using PruebaIngresoBibliotecario.Api.Infraestructure;
using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Api.DTOs;
using PruebaIngresoBibliotecario.Api.Domain.Contracts;

namespace PruebaIngresoBibliotecario.Api.Domain.Services
{
    public class PrestamoDomainService : IPrestamoDomainService
    {
        #region Fields
        private readonly PersistenceContext _context;
        #endregion

        #region Builder
        public PrestamoDomainService(PersistenceContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<int> CantidadDePrestamos(string identificacionUsuario)
        {
            var data = await _context.Prestamos
                         .Include(p => p.Usuario)
                         .CountAsync(p => p.Usuario.IdentificacionUsuario == identificacionUsuario);
            return data;
        }
        public async Task<Prestamo> GuardarDatos(PrestamoRequestDto data, DateTime fechaDevolucion)
        {
            var libro = await _context.Libros.FirstOrDefaultAsync(l => l.Isbn == data.Isbn);

            if (libro == null)
            {
                libro = new Libro
                {
                    Id = Guid.NewGuid(),
                    Isbn = data.Isbn
                };
                _context.Libros.Add(libro);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdentificacionUsuario == data.IdentificacionUsuario);

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    IdentificacionUsuario = data.IdentificacionUsuario,
                    TipoUsuario = data.TipoUsuario
                };
                _context.Usuarios.Add(usuario);
            }
            else
            {
                usuario.TipoUsuario = data.TipoUsuario;
                _context.Usuarios.Update(usuario);
            }

            Prestamo nuevoPrestamo = new Prestamo
            {
                Id = Guid.NewGuid(),
                LibroId = libro.Id,
                UsuarioId = usuario.Id,
                FechaPrestamo = DateTime.Now,
                FechaDevolucion = fechaDevolucion
            };

            _context.Prestamos.Add(nuevoPrestamo);
            await _context.SaveChangesAsync();

            return nuevoPrestamo;
        }

        public async Task<Prestamo> GetPrestamo(Guid idPrestamo)
        {
            return await _context.Prestamos
                .Include(x => x.Usuario)
                .Include(x => x.Libro)
                .FirstOrDefaultAsync(p => p.Id == idPrestamo);
        }
        #endregion
    }
}
