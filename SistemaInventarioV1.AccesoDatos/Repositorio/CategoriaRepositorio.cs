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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        //pasamos el dbContext
        private readonly ApplicationDbContext _db;
        //constructor para el dbContext
        public CategoriaRepositorio(ApplicationDbContext db) : base(db) //se paso al padre con : base(db) para evitar el error de referencia
        {
            _db = db;
        }

        public void Actualizar(Categoria categoria)
        {
            //variable para capturar el registro en base al id
            var categoriaBD = _db.Categorias.FirstOrDefault(b => b.Id == categoria.Id);
            if (categoriaBD != null)
            {
                categoriaBD.Nombre = categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado = categoria.Estado;
                _db.SaveChanges();
            }
        }
    }
}
