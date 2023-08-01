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
    public class BodegaProductoRepositorio : Repositorio<BodegaProducto>, IBodegaProductoRepositorio
    {
        //pasamos el dbContext
        private readonly ApplicationDbContext _db;
        //constructor para el dbContext
        public BodegaProductoRepositorio(ApplicationDbContext db) : base(db) //se paso al padre con : base(db) para evitar el error de referencia
        {
            _db = db;
        }

        public void Actualizar(BodegaProducto bodegaProducto)
        {
            //variable para capturar el registro en base al id
            var bodegaProductoBD = _db.BodegasProductos.FirstOrDefault(b => b.Id == bodegaProducto.Id);
            if (bodegaProductoBD != null)
            {
                bodegaProductoBD.Cantidad = bodegaProducto.Cantidad;
                _db.SaveChanges();
            }
        }
    }
}
