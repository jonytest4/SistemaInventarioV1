using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos.EspecificacionPag
{
    public class Metadata
    {
        //manejado del total de páginas
        public int TotalPages { get; set; }
        //
        public int PageSize { get; set; }
        //Maneja el total de páginas
        public int TotalCount { get; set; }
    }
}
