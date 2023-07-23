using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SistemaInventarioV1.AccesoDatos.Repositorio;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Modelos;
using SistemaInventarioV1.Utilidades;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    //Referenciar al área que pertenece
    [Area("Admin")]
    //etiqueta para autorización de acceso
    [Authorize(Roles = DS.RolAdmin)]
    public class MarcaController : Controller
    {
        //referenciamos la UnidadTrabajo creada del Repositorio Genérico
        private readonly IUnidadTrabajo _unidadTrabajo;
        //constructor para utilizar la unidad de trabajo
        public MarcaController (IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }
        //métodos upsert (Update-Insert) necesario para accesibilidad del bodega.js
        //carga datos a la vista
        public async Task<IActionResult> Upsert(int? id)
        {
            //variable del modelo 
            Marca marca = new Marca();

            if(id == null)
            {
                //crear nueva bodega
                marca.Estado = true;
                return View(marca);
            }
            else
            {
                //Actualización de bodega
                //para evitar el error de no se puede convertir int? a int después de id se agrega .GetValueOrDefault()
                marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
                if(marca == null)
                {
                    return NotFound();
                }
                return View(marca);
            }
        }
        //guarda datos
        [HttpPost]
        //etiqueta para evitar que un sitio cargado intente cargar datos falsificados en las solicitudes en nuestra página
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if(ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    //llenado del TempData de la vista parcial _notificaciones
                    TempData[DS.Exitosa] = "Marca creada Correctamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    //llenado del TempData de la vista parcial _notificaciones
                    TempData[DS.Exitosa] = "Marca actualizada Correctamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            //llenado del TempData de la vista parcial _notificaciones
            TempData[DS.Exitosa] = "Error al Grabar la Categoría";
            return View(marca);
        }

        //generar comentarios y separación de código
        #region API
        //verbo para la API
        //IActionResult: retorna la vista y objetos con formato json
        //Obtener registros
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            //variable para capturar todos los registros
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            //data es el nombre de referencia para javascript
            return Json(new {data = todos});
        }
        //Eliminar Registros
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDb = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDb == null)
            {
                //success nombre que se le da a a variable para ser tomada en marca.js
                return Json(new { success = false, message = "Error al eliminar Marca"});
            }
            _unidadTrabajo.Marca.Remover(marcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca eliminada exitosamente"});
        }
        //validacióne
        //dataanotation para que pueda ser invocado desde un archivo javaScript por el nombre de la acción
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            //valida y envía la información a la vista Upsert
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }
        #endregion
    }
}
