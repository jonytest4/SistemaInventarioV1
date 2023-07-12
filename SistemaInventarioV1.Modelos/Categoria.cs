using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos
{
    //configurar el archivo CategoriaConfiguración para la migración en AccesiDatos/Configuración
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre Requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe contener máximo 60 Caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripción Requerido")]
        [MaxLength(100, ErrorMessage = "Descripcion debe contener máximo 100 Caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Estado Requerido")]
        public bool Estado { get; set; }
    }
}
