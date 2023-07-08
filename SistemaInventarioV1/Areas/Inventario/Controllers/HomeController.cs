using Microsoft.AspNetCore.Mvc;
using SistemaInventarioV1.Modelos.ViewModels;
using System.Diagnostics;

namespace SistemaInventarioV1.Areas.Inventario.Controllers
{
    //linea para agregar el etiquedado de a que area pertenece el controlador//
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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