using Microsoft.AspNetCore.Mvc;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Modelos;
using SistemaInventarioV1.Modelos.ErrorViewModels;
using SistemaInventarioV1.Modelos.EspecificacionPag;
using System.Diagnostics;

namespace SistemaInventarioV1.Areas.Inventario.Controllers
{
    //linea para agregar el etiquedado de a que area pertenece el controlador//
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //inicialización de la unidad de trabajo para listar los productos
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }
        //Index metodo de paginación y búsqueda
        public IActionResult Index(int pagNumber = 1, string busqueda="", string busquedaActual="")
        {
            //validación de busqueda
            if (!String.IsNullOrEmpty(busqueda))
            {
                pagNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;
            //validaciones para la paginación
            if(pagNumber < 1)
            {
                pagNumber = 1;
            }
            //instancia de parámetros
            Parametros parametros = new Parametros()
            {
                PagNumber = pagNumber,
                //PageSize se puede cambiar para mostrar más productos en la página principal
                PageSize = 4
            };
            //variable para cargar los datos necesarios en memoria
            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);
            //caragar datos de busqueda en la variable resultado
            if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Descripcion.Contains(busqueda));
            }
            ViewData["TotalPaginas"] = resultado.metadata.TotalPages;
            ViewData["TotalRegistros"] = resultado.metadata.TotalCount;
            ViewData["PageSize"] = resultado.metadata.PageSize;
            ViewData["PageNumber"] = pagNumber;
            ViewData["Previo"] = "disabled"; // clase css para desactivar botones
            ViewData["Siguiente"] = "";

            if(pagNumber > 1)
            {
                ViewData["Previo"] = "";
            }
            if(resultado.metadata.TotalPages <= pagNumber) {
                ViewData["Siguiente"] = "disabled";
            }
            return View(resultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}