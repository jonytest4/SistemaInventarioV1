using Microsoft.EntityFrameworkCore;
using SistemaInventarioV1.AccesoDatos.Data;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Modelos.EspecificacionPag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Repositorio
{
    //Indicamos que el repositorio hará referencia al IRepositorio agregando public ... <T> : IRepositorio<T> where T : class
    //e implementamos todos los métodos de la interface aquí
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //creamos variable del tipoDbContext
        private readonly ApplicationDbContext _db;
        //proiedad genérica para trabajar
        internal DbSet<T> dbSet;
        //constructor de la clase para poder usarlo
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //equivale a un insert into table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //equivale a un Select * from (solo por id)
        }
        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            //conf propiedad de consulta
            IQueryable<T> query = dbSet;
            //trbajamos con cada parámetro
            //filtro
            if(filtro != null)
            {
                query = query.Where(filtro); //equivalente a un Select * from where ...
            }
            //incluir propiedades
            if (incluirPropiedades != null)
            {
                foreach (var incluirPro in incluirPropiedades.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPro);  //el include retorn las propiedades relacionadas al objeto ejemplo categoría,marca
                }
            }
            //orderBy
            if(orderBy != null)
            {
                query = orderBy(query);

            }
            //isTracking
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }
        public PagesList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            //conf propiedad de consulta
            IQueryable<T> query = dbSet;
            //trbajamos con cada parámetro
            //filtro
            if (filtro != null)
            {
                query = query.Where(filtro); //equivalente a un Select * from where ...
            }
            //incluir propiedades
            if (incluirPropiedades != null)
            {
                foreach (var incluirPro in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPro);  //el include retorn las propiedades relacionadas al objeto ejemplo categoría,marca
                }
            }
            //orderBy
            if (orderBy != null)
            {
                query = orderBy(query);

            }
            //isTracking
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return PagesList<T>.ToPagesList(query, parametros.PagNumber, parametros.PageSize);
        }
        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            //conf propiedad de consulta
            IQueryable<T> query = dbSet;
            //trbajamos con cada parámetro
            //filtro
            if (filtro != null)
            {
                query = query.Where(filtro); //equivalente a un Select * from where ...
            }
            //incluir propiedades
            if (incluirPropiedades != null)
            {
                foreach (var incluirPro in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPro);  //el include retorn las propiedades relacionadas al objeto ejemplo categoría,marca
                }
            }
            //isTracking
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }

        
    }
}
