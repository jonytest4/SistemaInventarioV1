using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio
{
    //configuramos similar a las otras interfaces
    //heradamos IDisposable se deshace de objetos que consuman recursos y ya no se esten utilizando
    //esta interface envuelve los repositorios que tenemos
    public interface IUnidadTrabajo : IDisposable 
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        IProductoRepositorio Producto { get; }
        Task Guardar();
    }
}
