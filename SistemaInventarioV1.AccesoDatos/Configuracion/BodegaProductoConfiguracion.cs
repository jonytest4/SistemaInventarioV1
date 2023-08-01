using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventarioV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.AccesoDatos.Configuracion
{
    //Contenido del fluentAPI que nos permite abarcar más características
    //heredamos IEntityTypeConfiguration para un tipo de modelo
    //configurar DBContext
    public class BodegaProductoConfiguracion : IEntityTypeConfiguration<BodegaProducto>
    {
        public void Configure(EntityTypeBuilder<BodegaProducto> builder)
        {
            //un builder por cada propiedad del modelo se incluye las condiciones
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BodegaId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();
           

            /*Relaciones incluse su cardinalidad*/
            //de Uno a muchos
            builder.HasOne(x => x.Bodega).WithMany().HasForeignKey(x => x.BodegaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Producto).WithMany().HasForeignKey(x => x.ProductoId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
