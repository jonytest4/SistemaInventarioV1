using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 //DS (definiciones estáticas)
namespace SistemaInventarioV1.Utilidades
{
    //DS (definiciones estáticas) declaración de constantes a utilizar en todo el proyecto
    public static class DS
    {
        //Constantes a ser utilizadas en visata parcial notificaciones
        public const string Exitosa = " Exitosa";
        public const string Error = "Error";

        //Constante para acceder al directorio donde se guardarán las imágenes
        public const string ImagenRuta = @"\img\producto\";
        //Constante para dar roles a los usuarios
        public const string RolAdmin = "Admin";
        public const string RolCliente = "Cliente";
        public const string RolInventario = "Inventario";
    }
}
