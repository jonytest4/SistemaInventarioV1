using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Modelos.EspecificacionPag
{
    //clase principal para el manejo de la paginación
    //clase genércia
    //revisar su implementación en IRepositrorio y Repositorio
    public class PagesList<T> : List<T>
    {
        public Metadata metadata { get; set; }
        //el constructor contiene las propiedades para controlar la lista
        public PagesList(List<T> items, int count, int pageNumber, int pageSize) {
            metadata = new Metadata()
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count/ (double)pageSize) //por ejemplo 1.5 lo transforma a 2
            };
            AddRange(items); //agrega los elementos de la colección al final de la lista

        }
        //método para usarlo en la paginación//
        public static PagesList<T> ToPagesList(IEnumerable<T> entidad, int pageNumber, int pageSize) {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumber -1)*pageSize).Take(pageSize).ToList();
            return new PagesList<T>(items, count, pageNumber, pageSize);
        }
    }
}
