using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos
{
    public class Bodega
    {
        //uso de data anotations
        //creado el modelo se ajusta el dbcontext
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre Requerido")]
        [MaxLength(60,ErrorMessage = "Nombre debe ser máximo 60 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripción Requerido")]
        [MaxLength(100, ErrorMessage = "Nombre debe ser máximo 100 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Estado Requerido")]
        public bool Estado { get; set; }
    }
}
