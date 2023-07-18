using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos.EspecificacionPag
{
    //Estas propiedades me ayudan para la paginación del despliegue de los producto pag principal
    public class Parametros
    {
        //número de la página
        public int PagNumber { get; set; } = 1 ;
        //tamaño de la página
        public int PageSize { get; set; } = 4;
    }
}
