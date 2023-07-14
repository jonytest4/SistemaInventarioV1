let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        //cambio del lenguaje a español de la librería DataTable
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Producto/ObtenerTodos"
        },
        "columns": [
            { "data": "numeroSerie"},
            { "data": "descripcion" },
            //aqui gracias al incluir propiedades del controlador me permite navegar y tomar el nombre de categoria y marca
            { "data": "categoria.nombre" },
            { "data": "marca.nombre" },
            {
                "data": "precio", "className": "text-end",
                "render": function (data) {
                    //validación del precio mediante expresiones regulares
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d;
                }
            },
            {
                "data": "costo", "className": "text-end",
                "render": function (data) {
                    //validación del precio mediante expresiones regulares
                    var c = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return c;
                }
            },
            {
                "data": "estado",
                "render": function (data) {
                    if (data == true) {
                        return "Activo";
                    }
                    else {
                        return "Inactivo";
                    }
                },
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                           <a href="/Admin/Producto/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                           <a onclick=Delete("/Admin/Producto/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                           </a> 
                        </div>
                    `;
                }, "width": "20%"
            }
        ]

    });
}
//generar la función de llamada del onclick=Delete
function Delete(url) {
    //invocación de la librería sweetalert
    swal({
        title: "¿Esta seguro de eliminar el Producto?",
        text: "Este registro no se podrá recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        //librería toast para enviar notificaciones
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

/*//generación de variable
let datatable
//cargar una función al cargar el docuemnto a la vista
$(document).ready(function () {
    loadDataTable(); // nombre de la función puede ser otro

});

//creación de funciones
function loadDataTable() {
    //captura del id de la tabla en la vista Razer en la variable creada, referenciamos el id creado para la tabla en el index ejemplo tblsDatos
    //y referenciamos la librería en este caso DataTable
    datatable = $('#tblDatos').DataTable({
        //secciónes de ajax
        "ajax": {
            //agregamos la dirección de nuestro método a utilizar del controlador
            "url":"/Admin/Bodega/ObtenerTodos"
        },
        //renderizado de las columnas
        "colums": [
            //referenciamos el nombre que colocamos en el controlador del metodo obtener todos
            //en este caso data 
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",
                //renderizado para obtener como palabra si esta activo o inactivo
                "render": function (data) { //pasamos data ya que en este instante contiene a estado
                    if (data == true) {
                        return "Activo";
                    } else {
                        return "Inactivo";
                    }
                }, "width": "20%"
            },
            //renderizado de la columna vacía referenciando la id hacia los botones con range
            {
                "data": "id",
                "render": function (data) {
                    // ` ` esta comillas me permiten renderizar código html en el render
                    return `
                        <div class ="text-center">
                            <a href="/Admin/Bodega/Upsert/${data}" class="btn btn-succes text-white" style="cursor:pointer">
                            <i class="bi bi-pencil-square"></i> 
                            </a>
                            <a onclick=Delete("/Admin/Bodega/Delete/${data}" class="btn btn-danger text-white" style="cursor:pointer")>
                            <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `;
                }, "width":"20%"
            }
        ]
    });
}*/