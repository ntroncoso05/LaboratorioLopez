var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadPaciente').DataTable({
        "ajax": {
            "url": "/api/Paciente",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nombre", "width": "10%" },
            { "data": "apellido", "width": "10%" },
            { "data": "direccion", "width": "25%" },
            { "data": "telefono", "width": "12%" },
            {
                'data': 'fechaNacimiento',
                'render': function (fecha) {
                    var date = new Date(fecha);
                    return date.toLocaleDateString('en-GB');
                }, "class": "text-center", "width": "18%",
            }, 
            {
                "data": "pacienteId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Factura/Inicio?id=${data}" class='btn btn-link text-blue' style='cursor:pointer; width:75px;'> 
                                    Facturar
                                </a>
                                <a href="/ListaPaciente/UpsertPaciente?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:75px;'> 
                                    Editar
                                </a>
                                <a class='btn btn-danger text-white' style='cursor:pointer; width:75px;' onClick=Delete('/api/Paciente?id='+${data})> 
                                    Eliminar
                                </a>
                            </div>`;
                }, "width": "75%"
            }
        ],
        "language": {
            "emptyTable": "No registro encontrado",
            "lengthMenu": "Mostrar _MENU_ registros por pag.",
            "zeroRecords": "No Registro",
            "info": "Mostrando pag. _PAGE_ de _PAGES_",
            "infoEmpty": "No registros disponible",
            "infoFiltered": "(filtrado de un total de _MAX_ registro)"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Estas Seguro/a?",
        text: "Al eliminar, no podra recuperar el record.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {     /*Response*/
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}