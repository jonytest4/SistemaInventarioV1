using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        [MaxLength(60, ErrorMessage = "El nombre debe tener máximo 60 Caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        [MaxLength(100, ErrorMessage = "La Descripción debe tener máximo 100 Caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Estado requerido")]
        public bool Estado { get; set; }
    }
}
