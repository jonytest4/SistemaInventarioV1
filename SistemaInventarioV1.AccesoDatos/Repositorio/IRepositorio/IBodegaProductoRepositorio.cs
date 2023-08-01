using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventarioV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio
{
    //modificamos a publico y agregamos : IRepositorio<Bodega>, Bodega es nuestro modelo como objeto
    public interface IBodegaProductoRepositorio : IRepositorio<BodegaProducto>
    {
        //metodo para actualizar
        void Actualizar(BodegaProducto bodegaProducto);
        
    }
}
