var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadPagoAseguradora').DataTable({
        "ajax": {
            "url": "/api/PagoAseguradora",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "facturaId", "width": "10%" },            
            {
                'data': 'fechaRegistro',
                'render': function (d) {
                    var date = new Date(d);
                    return (date.toDateString())
                }, "width": "15%"
            },
            {
                "data": "pagoId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a class='btn btn-danger text-white' style='cursor:pointer; width:75px;' onClick=Delete('/api/Paciente?id='+${data})> 
                                    Seleccionar
                                </a>
                            </div>`;
                }, "width": "85%"
            }
        ],
        //"language": {
        //    "emptyTable": "No registro encontrado",
        //    "lengthMenu": "Mostrar _MENU_ registros por pag.",
        //    "zeroRecords": "No Registro",
        //    "info": "Mostrando pag. _PAGE_ de _PAGES_",
        //    "infoEmpty": "No registros disponible",
        //    "infoFiltered": "(filtrado de un total de _MAX_ registro)"
        //},
        //"width": "100%"
    });
}