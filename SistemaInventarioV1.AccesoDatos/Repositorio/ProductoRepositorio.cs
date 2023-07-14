using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventarioV1.AccesoDatos.Data;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio
{
    //configuramos y agregamos : Repositorio<Bodega>, IBodegaRepositorio, tomando en cuenta que repositorio es genérico
    //e IBodegaRepositorio contendrá el actualizar
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        //pasamos el dbContext
        private readonly ApplicationDbContext _db;
        //constructor para el dbContext
        public ProductoRepositorio(ApplicationDbContext db) : base(db) //se paso al padre con : base(db) para evitar el error de referencia
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            //variable para capturar el registro en base al id
            var productoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (productoBD != null)
            {
                //validaciónde actualización de la imagen si se encuentra bajo modificación
                if (productoBD.ImagenUrl != null)
                {
                    productoBD.ImagenUrl = producto.ImagenUrl;
                }
                productoBD.NumeroSerie = producto.NumeroSerie;
                productoBD.Descripcion = producto.Descripcion;
                productoBD.Costo = producto.Costo;
                productoBD.Precio = producto.Precio;
                productoBD.CategoriaId = producto.CategoriaId;
                productoBD.MarcaId = producto.MarcaId;
                productoBD.PadreId = producto.PadreId;
                productoBD.Estado = producto.Estado;
                _db.SaveChanges();
            }
        }
        //obtención de los objetos para la lista de ProductoVM
        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if(obj == "Categoria")
            {
                //usar SelectListItem ya que usa dos atributos Text, Value
                return _db.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if(obj == "Producto")
            {
                return _db.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
