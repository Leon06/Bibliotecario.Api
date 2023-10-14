using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Api.Infraestructure
{
    public class Libro
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Isbn { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; } // Relacion uno a muchos con Prestamo
    }
}
