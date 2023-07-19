using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos
{
    //Modelo para usar la administración de usuarios
    //hereda del modelo ya creado por identity
    public class UsuarioApp : IdentityUser
    {
        //propiedades extras
        [Required(ErrorMessage ="Los nombres son requeridos")]
        [MaxLength(100)]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [MaxLength(100)]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "La dirección es requerida")]
        [MaxLength(200)]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La ciudad es requerido")]
        [MaxLength(60)]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El países requerido")]
        [MaxLength(60)]
        public string Pais { get; set; }
        //dataanotation para que la prpiedad no se agrege como columna en la base de datos
        [NotMapped]
        public string Rol { get; set; }
        //agregar al DbContext
    }
}
