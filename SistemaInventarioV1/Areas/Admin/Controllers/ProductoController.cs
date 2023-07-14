using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventarioV1.AccesoDatos.Repositorio;
using SistemaInventarioV1.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.Modelos;
using SistemaInventarioV1.Modelos.ViewModels;
using SistemaInventarioV1.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    //Referenciar al área que pertenece
    [Area("Admin")]
    public class ProductoController : Controller
    {
        //referenciamos la UnidadTrabajo creada del Repositorio Genérico
        private readonly IUnidadTrabajo _unidadTrabajo;
        //refecia para accesibilidad al directorio de imágenes
        private readonly IWebHostEnvironment _webHostEnvironment;
        //constructor para utilizar e inicializar la unidad de trabajo u otra propiedad
        public ProductoController (IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //métodos upsert (Update-Insert) necesario para accesibilidad del producto.js
        //carga datos a la vista
        public async Task<IActionResult> Upsert(int? id)
        {
            //creo el objeto del ProductoVM e inicializo sus propiedades
            ProductoVM productoVM = new ProductoVM()
            {
                producto = new Producto(),
                CategoriaLista =  _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Marca"),
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto")
            };

            if (id == null)
            {
                //Crear nuevo Producto
                return View(productoVM);
            }
            else{
                //Actualizar Producto
                productoVM.producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }
        //guarda datos
        [HttpPost]
        //etiqueta para evitar que un sitio cargado intente cargar datos falsificados en las solicitudes en nuestra página
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                //caprtura todo lo que se trae del form especialmente los archivos
                var files = HttpContext.Request.Form.Files;
                //captura de la ruta del archivo imagen
                string webRootPath = _webHostEnvironment.WebRootPath;
                //verificación de insert o update
                if(productoVM.producto.Id == 0)
                {
                    //Crear
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString(); //Guid identificador únido que se le agrega a la imagen
                    string extension = Path.GetExtension(files[0].FileName);//captura de la extensión del archivo que se está recibiendo

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),FileMode.Create)) //creación de la imagen
                    {
                        files[0].CopyTo(fileStream); //almacenamiento de la imagen física en memoria
                    }
                    productoVM.producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.producto);
                }
                else
                {
                    //actualizar
                    var objetoProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.producto.Id, isTracking:false); //isTrackin  previene que consulte el objeto y lo pueda modificar
                    //validación si es o no nueva imagen
                    if (files.Count > 0) // Si carga nueva imagen para el producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        /*borrar imagen*/
                        var anterioFile = Path.Combine(upload, objetoProducto.ImagenUrl);
                        //validació si existe
                        if (System.IO.File.Exists(anterioFile))//manejo de archivos del sistema con System, líne que verificara si existe el archivo
                        {
                            System.IO.File.Delete(anterioFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.producto.ImagenUrl = fileName + extension;
                    }
                    else// si no se carga una nueva imagen
                    {
                        productoVM.producto.ImagenUrl = objetoProducto.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.producto);
                }
                //Notificaciones y guardado
                TempData[DS.Exitosa] = "Transacción Exitosa";
                await _unidadTrabajo.Guardar();
                return View("Index");
            }
            //si el modelo no es válido
            productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Categoria");
            productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Marca");
            productoVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("Producto");
            return View(productoVM);
        }

        //generar comentarios y separación de código
        #region API
        //verbo para la API
        //IActionResult: retorna la vista y objetos con formato json
        //Obtener registros
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            //variable para capturar todos los registros e includPropiedades para las propiedades de navegación del modelo y la tabla
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca");
            //data es el nombre de referencia para javascript
            return Json(new {data = todos});
        }
        //Eliminar Registros
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null)
            {
                //success nombre que se le da a a variable para ser tomada en marca.js
                return Json(new { success = false, message = "Error al eliminar Producto"});
            }
            //Remover la imagen física antes de remover el producto
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anterioFile = Path.Combine(upload, productoDb.ImagenUrl);
            if (System.IO.File.Exists(anterioFile)) //saber si el archivo físicamente se encuentra
            {
                System.IO.File.Delete(anterioFile);
            }
            //
            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto eliminada exitosamente"});
        }
        //validacióne
        //dataanotation para que pueda ser invocado desde un archivo javaScript por el nombre de la acción
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string serie, int id = 0)
        {
            
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
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
