var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadSeguro').DataTable({
        "ajax": {
            "url": "/api/Seguro",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nombre", "width": "10%" },
            { "data": "rnc", "width": "10%" },
            { "data": "direccion", "width": "25%" },
            { "data": "telefono", "width": "15%" },
            { "data": "email", "width": "15%" },
            {
                "data": "seguroId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/ListaSeguro/UpsertSeguro?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:75px;'> 
                                    Editar
                                </a>
                                <a class='btn btn-danger text-white' style='cursor:pointer; width:75px;' onClick=Delete('/api/Seguro?id='+${data})> 
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