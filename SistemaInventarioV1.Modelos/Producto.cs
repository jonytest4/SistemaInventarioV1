using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos
{
    public class Producto
    {
        [Key] 
        public int Id { get; set; }
        [Required(ErrorMessage = "Número de serie Requerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }
        [Required(ErrorMessage = "Descripción Requerida")]
        [MaxLength(100)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Precio Requerido")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "Costo Requerido")]
        public double Costo { get; set; }
        public string ImagenUrl { get; set; }
        [Required(ErrorMessage = "Estado Requerido")]
        public bool Estado { get; set; }
        [Required(ErrorMessage = "Categoría es Requerida")]
        public int CategoriaId { get; set; }
        //relación de la tabla categoría en el modelo producto (navegación)
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set;}
        [Required(ErrorMessage = "Marca es Requerida")]
        public int MarcaId { get; set; }
        //relación de la tabla marca en el modelo producto (navegación)
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set;}
        //Relación de recursividad para identificar la propiedad padre(un producto puede relacionarse a un mismo producto)
        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }
    }
}
