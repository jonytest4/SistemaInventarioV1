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
    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
    {
        //pasamos el dbContext
        private readonly ApplicationDbContext _db;
        //constructor para el dbContext
        public BodegaRepositorio(ApplicationDbContext db) : base(db) //se paso al padre con : base(db) para evitar el error de referencia
        {
            _db = db;
        }

        public void Actualizar(Bodega bodega)
        {
            //variable para capturar el registro en base al id
            var bodegaBD = _db.Bodegas.FirstOrDefault(b => b.Id == bodega.Id);
            if (bodegaBD != null)
            {
                bodegaBD.Nombre = bodega.Nombre;
                bodegaBD.Descripcion = bodega.Descripcion;
                bodegaBD.Estado = bodega.Estado;
                _db.SaveChanges();
            }
        }
    }
}
