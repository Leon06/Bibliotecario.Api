using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Infraestructure
{
    public class Prestamo
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Libro))]
        public Guid LibroId { get; set; }
        public virtual Libro Libro { get; set; } // Relacion muchos a uno con Libro

        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; } // Relacion muchos a uno con Usuario

        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
    }
}
