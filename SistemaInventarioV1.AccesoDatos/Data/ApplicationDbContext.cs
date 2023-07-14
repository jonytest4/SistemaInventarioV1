using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioV1.Modelos;
using System.Reflection;
//referenciar correctamente el namespace en este caso se referencia al proyecto AccesoDatos **//
namespace SistemaInventarioV1.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //lineas para incorporar los modelos a la base de datos
        //Se agrega cada nuevop modelo una ves configurado el archivo de configuración de FLuent API
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }

        //override para el FLuentAPI para cambiar sus caracteristicas//
        //permitirá que nuestros archivos de configuración creados  haga un override de lo que actualmente existe
        //y tomará esa configuración del directorio Configuracion en el proyecto AccesoDatos
        // directorio Configuracion de cada aparatado ejemplo BodegaConfiguracion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}