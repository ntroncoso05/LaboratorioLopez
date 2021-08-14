var dataTable;
var rowIdx = 0; 
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadExamen').DataTable({
        "ajax": {
            "url": "/api/PacienteFactura",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoria", "width": "15%" },
            { "data": "descripcion", "width": "55%" },
            {
                'data': 'precio',
                'render': function (precio) {
                    return 'RD$' + precio.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                }, "width": "15%"
            },
            {
                "data": "examenId",
                "render": function (data) {
                    return `<div class="text-center">
                                <a class='btn btn-primary text-white' style='cursor:pointer; width:40px;' onClick=Agregar('/api/PacienteFactura?id='+${data})> 
                                    +
                                </a>
                            </div>`;
                }, "width": "15%"
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

function Agregar(url) { 
    $.ajax({
        type: "Post",       
        url: url,
        success: function (data) {
            // Agrega una fila nueva adentro del cuerpo (tbody). 
            var tablaCuerpo = document.getElementById('tbody');
            var nuevaFila = true;
            for (var i = 0; i < tablaCuerpo.rows.length; i++) {
                if (data.dataValor.examenId == parseInt(tablaCuerpo.rows[i].cells[1].innerText)) {
                    nuevaFila = false;
                }
            }

            if (nuevaFila || tablaCuerpo.rows.length == 0) {
                $('#tbody').append(`<tr id="R${++rowIdx}"> 

			    <td class="fila-indice text-center"> 
			        <p>${rowIdx}</p> 
			    </td> 

                <td class="text-center"> 
				    <p>${data.dataValor.examenId}</p>
				</td>

			    <td class="text-center"> 
				    <p>${data.dataValor.descripcion}</p>
				</td>

                <td class="text-center"> 
				    <p>${"RD$" + data.dataValor.precio.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}</p>
				</td>

			   <td class="text-center"> 
				    <p><input id="inputPorcentaje${rowIdx}" type='number' class='inputPorcentaje text-center' style='width:50px;'>
                            <style>
                                input::-webkit-outer-spin-button,
                                input::-webkit-inner-spin-button {
                                  -webkit-appearance: none;
                                  margin: 0;
                                }
                            </style></p>
				</td>

                <td class="text-center"> 
				    <p><input id="inputPago${rowIdx}" type='number' class='inputPago text-center' style='width:50px;'>
                            <style>
                                input::-webkit-outer-spin-button,
                                input::-webkit-inner-spin-button {
                                  -webkit-appearance: none;
                                  margin: 0;
                                }
                            </style></p>
				</td>

                <td class="celda-monto text-center">
			        <p>${"RD$" + (0).toFixed(2)}</p> 
			    </td>

                <td class="text-center"> 
				    <p><a class='removerExamen btn btn-danger text-white' style='cursor:pointer; width:40px;'>-</a ></p>
				</td>
                
			    </tr>`);
                toastr.success(data.message);
                document.getElementById("inputPago" + rowIdx).value = data.dataValor.precio;
                calcularTotales();
            }
            else {
                toastr.warning(`Examen ${data.dataValor.descripcion} duplicado no agregado`);
            }            
        }
    });
}

$('#tfoot').append(
    `<tr>
        <td colspan="2" class="text-sm-center">
            <a  class=' btn btn-success text-white facturar' style='cursor:pointer; width:80px;' onClick=GuardarFactura()>Facturar</a>
        </td>
    </tr>

    <tr>
        <td class="text-left"><strong>Monto:</strong></td>
        <td class="precioTotal text-center">RD$0.00</td>
    </tr>

    <tr>
        <td class="text-left"><strong>Cobertura:</strong></td>
        <td class="porcentajePromedio text-center">RD$0.00</td>
    </tr>

    <tr>
        <td class="text-left"><strong>Pago:</strong></td>
        <td class="porcentajePromedio text-center">RD$0.00</td>
    </tr>

    <tr>
        <td class="text-left"><strong>Balance:</strong></td>
        <td class="montoTotal text-center">RD$0.00</td>
    </tr>

    <tr>
        <td class="text-left"><strong>RD$:</strong></td>
        <td class="text-center">
            <input type='number' class="inputDevuelta" title='Entrar pago para calculo devuelta' style='width:100px;' />
                <style>
                    input::-webkit-outer-spin-button,
                    input::-webkit-inner-spin-button 
                    {
                        -webkit-appearance: none;
                        margin: 0;
                    }
                </style>
        </td>
    </tr>

    <tr>
        <td class="text-left"><strong>Cambio:</strong></td>
        <td class="text-center">RD$0.00</td>
    </tr>                 
`);

function calcularTotales() {
    // Actualiza el pie de la tabla cuando el % cambia y al agregar/remover una fila.
    var tablaCuerpo = document.getElementById('tbody'), sumaTotalPrecio = 0, sumaTotalPago = 0, sumaTotalMonto = 0;

    // Suma las lineas en la tabla factura para los Totales.
    for (var i = 0; i < tablaCuerpo.rows.length; i++) {
        sumaTotalPago += parseFloat(document.getElementById('inputPago' + (i + 1)).value);
        sumaTotalPrecio += parseInt(tablaCuerpo.rows[i].cells[3].innerText.replace(/[RD$]|,/g, ""));
        sumaTotalMonto += parseInt(tablaCuerpo.rows[i].cells[6].innerText.replace(/[RD$]|,/g, ""));
    }
    
    var tablaPie = document.getElementById('tfoot');
    tablaPie.rows[1].cells[1].innerText = `RD$${sumaTotalPrecio.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
    tablaPie.rows[3].cells[1].innerText = `RD$${sumaTotalPago.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
    tablaPie.rows[4].cells[1].innerText = `RD$${sumaTotalMonto.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
    tablaPie.rows[2].cells[1].innerText = `RD$${(sumaTotalPrecio - sumaTotalMonto - sumaTotalPago).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
}

function GuardarFactura() {
    // Variable almacena los datos de la tabla DT_loadFactura 
    var tablaCuerpo = document.getElementById('tbody');    

    //Crea una lista de objetos, cada linea un objeto con propiedades del modelo PagoFactura.
    var valoresTabla = [];
    for (var i = 0; i < tablaCuerpo.rows.length; i++) {
        valoresTabla[i] = {},
            valoresTabla[i].LineaFactura = parseInt(tablaCuerpo.rows[i].cells[0].innerText),
            valoresTabla[i].PacienteId = parseInt($('#labelPacienteId').val()),
            valoresTabla[i].ExamenId = parseInt(tablaCuerpo.rows[i].cells[1].innerText),
            valoresTabla[i].ExamenPrecio = parseFloat(tablaCuerpo.rows[i].cells[3].innerText.replace(/[RD$]|,/g, "")),
            valoresTabla[i].PagoPaciente = parseFloat(document.getElementById('inputPago' + (i + 1)).value.replace(/[RD$]|,/g, "")),
            valoresTabla[i].PagoSeguro = Math.ceil(parseFloat(tablaCuerpo.rows[i].cells[3].innerText.replace(/[RD$]|,/g, "") * document.getElementById('inputPorcentaje' + (i + 1)).value)/100),
            valoresTabla[i].Balance = parseFloat(tablaCuerpo.rows[i].cells[6].innerText.replace(/[RD$]|,/g, ""));
        //valoresTabla[i].SeguroId = parseInt(dropDownListSeguro.options[dropDownListSeguro.selectedIndex].value);
    };
    valoresTablaJS = JSON.stringify( valoresTabla );

    $.ajax({
        type: "POST",        
        data: valoresTablaJS,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        url: "/api/FacturarDb",         
        success: function (data) {
            if (data.success) {
                swal({
                    closeOnClickOutside: false,
                    title: data.message,
                    text: "Seleccione: Regresar a Pacientes o Reportes",
                    icon: "success",
                    buttons: {
                        Volver: true,
                        Reportes: "Reportes",
                    },
                }).then((value) => {
                    switch (value) {
                        case "Reportes":
                            window.location.href = 'https:/Reportes/Factura';
                            break;
                        default:
                            window.location.href = 'https:/ListaPaciente/Index';
                    }
                });
                    
            }
            else {
                toastr.error(data.message);
            }
        }
    });    
}

// jQuery botton click evento para remover fila. 
$('#tbody').on('click', '.removerExamen', function () {

    // Selecciona todas las filas incluyendo la fila seleccionada.
    var filaHija = $(this).closest('tr').nextAll();

    // Iterando por las filas de la tabla adentro del cuerpo (tbody).
    filaHija.each(function () {

        // Selecciona <tr> id. 
        var id = $(this).attr('id');

        // Selecciona el <p> dentro de .fila-indice class.
        var indice = $(this).children('.fila-indice').children('p');

        // Selecciona el numero de la fila de <tr> id. 
        var dig = parseInt(id.substring(1));

        // Modificar el indice de la fila row index. 
        indice.html(`${dig - 1}`);

        // Modifica fila id. 
        $(this).attr('id', `R${dig-1}`);
    });

    // Remover fila selecciona. 
    $(this).closest('tr').remove();

    // Reducir el total numeros de filas por 1. 
    --rowIdx;

    toastr.warning("Linea Factura Removida ");

    // Invoca la funcion para calcular los totales
    calcularTotales();
});

$('#tbody').on('input', '.inputPorcentaje', function () {
    // TODO - Valida que el rango de % este entre 0 - 100 y no contenga estos valores(e + -)
    if ($(this).val() > 100) {
        $(this).val('100');
    }
    if ($(this).val() < 0 || $(this).val() == "") {
        $(this).val('0');
    }
    // Variable contiene valores de la tabla
    var tabla = document.getElementById('DT_loadFactura');

    var filaHija = $(this).closest('tr')
    //Trae Id de la fila seleccionada.
    var id = filaHija.attr('id');

    //Convierte el id de la fila en int. 
    var dig = parseInt(id.substring(1));

    //Trae Id de la fila seleccionada para seleccionar o editar celdas.
    var rowSelected = tabla.getElementsByTagName('tr')[dig];

    //seleciona valor  de la celda/columna dentro de la fila .celda-monto.
    var indiceMonto = filaHija.children('.celda-monto').children('p');

    //Guarda el valor en html en la celda
    var cal = Math.ceil(parseFloat(rowSelected.cells[3].innerText.replace(/[RD$]|,/g, "")) - parseFloat(rowSelected.cells[3].innerText.replace(/[RD$]|,/g, "")) * ($(this).val() / 100))
    
    indiceMonto.html(`RD$${(0).toFixed(2)}`)

    document.getElementById("inputPago" + dig).value =cal
    calcularTotales();
});

$('#tbody').on('input', '.inputPago', function () {

    if ($(this).val() < 0 || $(this).val() == "") {
        $(this).val('0');
    }
    //Trae Id de la fila seleccionada para seleccionar o editar celdas.
    var numeroFila = parseInt($(this).closest('tr').attr('id').substring(1));
    var filaSeleccionada = document.getElementById('DT_loadFactura').getElementsByTagName('tr')[numeroFila];
    indiceMonto = $(this).closest('tr').children('.celda-monto').children('p');

    var porcentajeInput = document.getElementById('inputPorcentaje' + numeroFila).value;
    if (porcentajeInput == "")
        porcentajeInput = 0;
    var porcentajeCobertura = 1 - parseFloat(porcentajeInput) / 100;
    var balance = Math.ceil(parseFloat(filaSeleccionada.cells[3].innerText.replace(/[RD$]|,/g, "")) * porcentajeCobertura);

    if ($(this).val() > balance)
        $(this).val(balance);

    indiceMonto.html((`RD$${(balance- $(this).val()).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`))
    calcularTotales();
});

$('#tfoot').on('input', '.inputDevuelta', function () {
    var tablaPie = document.getElementById('tfoot');

    var devuelta = $(this).val() - tablaPie.rows[3].cells[1].innerText.replace(/[RD$]|,/g, "");
    devuelta = devuelta.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    tablaPie.rows[6].cells[1].innerText = `RD$${devuelta }`;
});