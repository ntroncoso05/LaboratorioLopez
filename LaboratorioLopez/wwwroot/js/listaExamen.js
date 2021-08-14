var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadExamen').DataTable({
        "ajax": {
            "url": "/api/Examen",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoria", "width": "20%" },
            { "data": "descripcion", "width": "35%" },
            {
                'data': 'precio',
                'render': function (precio) {
                    return 'RD$' + precio.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                }, "width": "10%" },            
            {
                "data": "examenId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/ListaExamen/UpsertExamen?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:75px;'> 
                                    Editar
                                </a>
                                <a class='btn btn-danger text-white' style='cursor:pointer; width:75px;' onClick=Delete('/api/Examen?id='+${data})> 
                                    Eliminar
                                </a>
                            </div>`;
                }, "width": "35%"
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