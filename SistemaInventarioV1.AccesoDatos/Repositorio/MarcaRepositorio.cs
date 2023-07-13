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
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        //pasamos el dbContext
        private readonly ApplicationDbContext _db;
        //constructor para el dbContext
        public MarcaRepositorio(ApplicationDbContext db) : base(db) //se paso al padre con : base(db) para evitar el error de referencia
        {
            _db = db;
        }

        public void Actualizar(Marca marca)
        {
            //variable para capturar el registro en base al id
            var marcaBD = _db.Marcas.FirstOrDefault(b => b.Id == marca.Id);
            if (marcaBD != null)
            {
                marcaBD.Nombre = marca.Nombre;
                marcaBD.Descripcion = marca.Descripcion;
                marcaBD.Estado = marca.Estado;
                _db.SaveChanges();
            }
        }
    }
}
