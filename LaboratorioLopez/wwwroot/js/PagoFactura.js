var dataTable;
var filaCheck=0;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadPagos').DataTable({
        "ajax": {
            "url": "/api/Pagos",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            /*{ "data": "pagoId" },*/
            {
                "render": function () {
                    return `<input id='R${filaCheck++}' type="checkbox"/>`
                }, "width": "1%"
            },
            {
                "render": function (a, b, data) {
                    return `<td class="text-center"> 
                    <p><label id="pagoIdLabel" class='pagoIdLabel'>${data.pagoId}</label></p>
                    
				</td>`;
                }, "width": "4%"
            },
            { "data": "pacienteNombre", "width": "27%" },

            { "data": "descripcion", "width": "35%" },
            {
                'data': 'pagoPaciente',
                'render': function (balance) {
                    return `<td class="text-center"> 
                    <p><label id="pagoLabel" class='pagoLabel'>${'RD$' + balance.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}</label></p>
                    
				</td>`
                }, "width": "10%"
            },
            //{
            //    'data': 'pagoPaciente',
            //    'render': function (balance) {
            //        return `<td class="text-center"> 
            //            <p><input id="inputPago" value='${balance.toFixed(2)}' style="width:100%" type='number' class='inputPago text-center'>
            //                <style>
            //                    input::-webkit-outer-spin-button,
            //                    input::-webkit-inner-spin-button { -webkit-appearance: none; margin: 0; }
            //                </style></p></td>`;
            //    }, "width": "10%"
            //},
            {
                'data': 'fechaRegistro',
                'render': function (fecha) {
                    var date = new Date(fecha);
                    return date.toLocaleDateString('en-GB');
                }, "width": "15%"
            },
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
    //dataTable.column($(this).attr('#Pago')).visible(!dataTable.column($(this).attr('#Pago')).visible());
}
$('#pagoButton').append(
    `<tr>
        <td colspan="2" class="text-sm-center">
            <a  class=' btn btn-info text-white' style='cursor:pointer; width:auto;' onClick=GuardarPago()>Procesar Pago</a>
        </td>
    </tr>`
);

function GuardarPago() {
    //var tablaCuerpo = document.getElementById('DT_loadPagos');
    //var checkBox;
    var indice = 0;

    // Variable almacena total filas de la tabla DT_loadFactura.
    var numeroFilas = $('#DT_loadPagos').DataTable().rows()[0];
    // Variable almacena colleccion datos de la tabla DT_loadFactura.
    var dataTable = $('#DT_loadPagos').DataTable();
    // Variable almacena colleccion de todos los input <chekbox> elementos.
    var todosCheckBox = dataTable.$('input');
    // Variable almacena colleccion de todos los PagosId columna.
    var todosPagosId = dataTable.$('#pagoIdLabel');

    // Variable almacena colleccion de todos los balance columna.
    var todosPagos = dataTable.$('#pagoLabel');

    //Crea una lista de objetos, cada linea un objeto con propiedades del modelo PagoFactura.
    var valoresPagos = [];
    for (var i = 0; i < numeroFilas.length; i++) {
        
        checkBox = document.getElementById("R" + i);
        if (todosCheckBox[i].checked == true) {
            //var dataTabla = $('#DT_loadPagos').DataTable().row(i).data();
            valoresPagos[indice] = {},
                valoresPagos[indice].PagoId = parseInt(todosPagosId[i].innerText),
                valoresPagos[indice].PagoPaciente = parseFloat(todosPagos[i].innerText.replace(/[RD$]|,/g, ""));
            indice++;
        };
    };
    var valoresPagosJS = JSON.stringify(valoresPagos);

    $.ajax({
        type: "POST",
        data: valoresPagosJS,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        url: "/api/Pagos",
        success: function (data) {
            if (data.success) {
                swal({
                    closeOnClickOutside: false,
                    title: data.message,
                    text: "Seleccione: Regresar a Pagos o Reportes",
                    icon: "success",
                    buttons: {
                        Volver: true,
                        Reportes: "Reportes",
                    },
                }).then((value) => {
                    switch (value) {
                        case "Reportes":
                            window.location.href = 'https:/Reportes/Pago';
                            break;                        
                        default:
                            dataTable.ajax.reload();
                    }
                });                
            }
            else { toastr.error(data.message); }
        }
    });    
}