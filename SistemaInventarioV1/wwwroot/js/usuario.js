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
            "url": "/Admin/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email" },
            { "data": "nombres"},
            { "data": "apellidos"},
            { "data": "phoneNumber" },
            { "data": "rol" },
            {
                //para trabajar con más columnas se abre llaves
                "data": {
                    id: "id", LockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    //variable para capturar la fecha actual del sistema y del bloqueo de usuario
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        //usuario esta bloqueado
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="bi bi-unlock"></i> Desbloquear
                               </a> 
                            </div>
                        `;
                    } else {
                        //usuario esta desbloqueado
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-lock"></i> Bloquear
                               </a> 
                            </div>
                        `; 
                    }
                }
            }
        ]

    });
}
//generar la función de llamada del onclick=Delete
function BloquearDesbloquear(id) {
    //invocación de la librería sweetalert
    $.ajax({
        type: "POST",
        url: "/Admin/Usuario/BloquearDesbloquear", //url del método
        data: JSON.stringify(id),//paso de solo el id por json
        contentType : "application/json",//indicar al programa que se está trabajando con json
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