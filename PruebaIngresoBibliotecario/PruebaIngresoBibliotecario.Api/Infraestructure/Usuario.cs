using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using PruebaIngresoBibliotecario.Api.Infraestructure.Enum;

namespace PruebaIngresoBibliotecario.Api.Infraestructure
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(10)]
        public string IdentificacionUsuario { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; } // Relacion uno a muchos con Prestamo
    }
}
