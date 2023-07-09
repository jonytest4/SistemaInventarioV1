using SistemaInventarioV1.AccesoDatos.Data;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        //**Nota se agrega esta unidad de trabajo como servicio revisar program.cs proyecto principal
        //creación de la propiedad dbContext similar a los otros repositorios
        private readonly ApplicationDbContext _db;
        //creación de propiedades para cada repositorio a trabajar
        public IBodegaRepositorio Bodega { get; private set; }
        //constructor
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
        } 

        public void Dispose()
        {
            _db.Dispose();
        }
        //guradado global
        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
