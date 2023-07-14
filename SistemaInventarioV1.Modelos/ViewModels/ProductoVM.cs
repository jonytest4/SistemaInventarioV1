using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos.ViewModels
{
    //Modificamos que sea público
    public class ProductoVM
    {
        //intanciamos el objeto producto mediante una propiedad
        public Producto producto { get; set; }
        //obtenemos las listas de categoría y marca
        //Instalar paquete mvc.ViewFeatures
        public IEnumerable<SelectListItem> CategoriaLista { get; set; }
        public IEnumerable<SelectListItem> MarcaLista { get; set; }
        //lista de recursividad, identificar si un producto pertence a un producto padre
        public IEnumerable<SelectListItem> PadreLista { get; set; }
        /* los métodos para el llenado de estas listas revisar Repositorios de producto*/
    }
}
