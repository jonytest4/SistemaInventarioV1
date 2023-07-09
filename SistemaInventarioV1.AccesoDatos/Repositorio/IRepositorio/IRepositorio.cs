using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio
{
    //modificamos a publico, agregamos <T> where T: class para hacerlo genérico y pueda recibir cualquier tipo de objto
    public interface IRepositorio<T> where T : class
    {
        //detallado de los métodos a utilizar
        //obtener un objeto
        Task<T> Obtener(int id);
        //obtener varios objetos
        Task<IEnumerable<T>> ObtenerTodos(
           //Expression funciona como filtro para filtrar la lista obtenida en base a un funcion con objeto y booleano
           Expression<Func<T, bool>> filtro = null,
           //Linea para ordenar la lista que en la funcion recibe los datos y los ordena
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           //linea que se encarga de hacer todos los enlaces con los otros objetos ejemplo categoría y marca
           string incluirPropiedades = null,
           //linea para acceder a una lista de objeto y en ese momento tambien se lo quiera modifcar
           bool isTracking = true
            );
        //obtener el primer objeto
        Task<T> ObtenerPrimero(
           Expression<Func<T, bool>> filtro = null,
           string incluirPropiedades = null,
           bool isTracking = true
            );
        //método para agregar nuevo objeto
        Task Agregar(T entidad);
        //método para remover el objeto
        void Remover(T entidad);
        //metodo para remover por rango (lista de objetos)
        void RemoverRango(IEnumerable<T> entidad);
        //** NOTA para que la interface sea asíncrona colocar Task ejemplo Task<T> al inicio de cada uno excepto los métodos de remover
        //** estos no pueden ser asíncronos
        //** para agregar se retira el void y se coloca task "void Agregar" > "Task Agregar"
    }
}
