using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioV1.AccesoDatos.Data;
using SistemaInventarioV1.AccesoDatos.Repositorio;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    //indicar al área al que pertenece
    [Area("Admin")]
    public class UsuarioController : Controller
    {
        //Intansiamos la unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;
        //acceso a los controles necesarios
        private readonly ApplicationDbContext _db;
        //por inyección de dependencias
        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioList = await _unidadTrabajo.UsuarioApp.ObtenerTodos();
            //uso del dbContext para obtener el rol de los usuarios
            var userRol = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            //recorrido de la lista para capturar el rol y asignarlo a una propiedad
            foreach (var usuario in usuarioList)
            {
                var roleId = userRol.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Rol = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new {data = usuarioList});
        }

        //método para el bloqueo de los usuarios
        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id) //FromBody para pasar información desde la vista
        {
            var usuario = await _unidadTrabajo.UsuarioApp.ObtenerPrimero(u => u.Id == id);
            if(usuario == null)
            {
                return Json(new { success = false, message = "Error de usuario"});
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                //usuario bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación exitosa" });
        }
        #endregion
    }
}
