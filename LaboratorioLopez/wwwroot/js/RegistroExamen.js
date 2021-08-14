var dataTable;
var filaCheck = 0;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_loadExamenPaciente').DataTable({
        "ajax": {
            "url": "/api/RegistroExamen",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
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

            {
                'render': function (a, b, data) {
                    return data.pacienteNombre + ' ' + data.pacienteApellido
                },"width": "20%"
            },
            { "data": "examenDescripcion", "width": "40%" },
            
            {
                "render": function (a, b, data) {                  
                    return `<td class="text-center"> 
                    <p><textarea id="examenResultado" class='examenResultado' cols="35" rows="1"></textarea></p>
                    
				</td>`;    
                }, "width": "25%"                
            },
            { "data": "dimensiones", "width": "5%", },
            {
                "render": function (d, mostrarTipo, data) {
                    //Llama la funcion LlenarDropDownListResultado() y almacena la lista para llenar drop down list.
                    var listaResultado = LlenarDropDownListResultado(data.parametros);                    
                    var $select = $("<select></select>", {
                        //"id": data[0] + "dropDownListResultado",
                        //"value": d
                    });
                    $.each(listaResultado, function (indiceNumero, textoValor) {
                        var $option = $("<option></option>", {
                            "text": textoValor,
                            "value": indiceNumero
                        });
                        //if (d === textoValor) {
                        //    $option.attr("selected", "selected")
                        //}
                        $select.append($option);
                    });
                    return $select.prop("outerHTML");
                }, "width": "5%"
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
    });

    //Ordenar tabla en la columna PacienteId.
    var table = $('#DT_loadExamenPaciente').DataTable();
    table.order([1, 'asc']).draw();

    //Oculta las columnas.
    //dataTable.column($(this).attr('#Pago')).visible(!dataTable.column($(this).attr('#Pago')).visible());

    //Evento boton click guardar resultado examen en base datos.
    guardarPacinteExamenes.onclick = function guardarPacinteResultado() { 
        
        // Variable almacena total filas de la tabla DT_loadFactura.
        var numeroFilas = $('#DT_loadExamenPaciente').DataTable().rows()[0];
        // Variable almacena colleccion datos de la tabla DT_loadFactura.
        var dataTable = $('#DT_loadExamenPaciente').DataTable();
        // Variable almacena colleccion de todos los drop down list <select> elementos.
        var todosDropDowns = dataTable.$('select');
        // Variable almacena colleccion de todos los input <text area> elementos.
        var todosTextArea = dataTable.$('textarea');
        // Variable almacena colleccion de todos los input <chekbox> elementos.
        var todosCheckBox = dataTable.$('input');
        // Variable almacena colleccion de todos los PagosId columna.
        var todasCeldas = dataTable.$('label');
        
        var indiceValoresTabla = 0;
        var dropDownResultado;

        //Crea una lista de objetos, cada linea un objeto con propiedades del modelo EntradaExamen.
        var valoresTabla = [];        
        for (var i = 0; i < numeroFilas.length; i++) {

            dropDownResultado = "";            
            dropDownValor = todosDropDowns[i].options[todosDropDowns[i].selectedIndex].value;
            if (dropDownValor > 0) {
                dropDownResultado = todosDropDowns[i].options[todosDropDowns[i].selectedIndex].text;
            }

            if (todosCheckBox[i].checked == true) {
                valoresTabla[indiceValoresTabla] = {},
                    valoresTabla[indiceValoresTabla].PagoId = parseInt(todasCeldas[i].innerText),
                    valoresTabla[indiceValoresTabla].ResultadoAnalisis = todosTextArea[i].value,
                    valoresTabla[indiceValoresTabla].ResultadoAnalisisParametro = dropDownResultado,
                indiceValoresTabla++
            };
        };
        //convierte la variable valoresTabla en un JSON string para ser pasado por la funcion ajax.
        valoresTablaJS = JSON.stringify(valoresTabla);

        //Pasa la valoresTablaJS al controlador MVC para ser guardada en la base datos.
        $.ajax({
            type: "POST",
            data: valoresTablaJS,
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            url: "/api/RegistroExamen",
            success: function (data) {
                if (data.success) {
                    swal({
                        closeOnClickOutside: false,
                        title: data.message,
                        text: "Seleccione: Regresar a Resultado o Reportes",
                        icon: "success",
                        buttons: {
                            Volver: true,
                            Reportes: "Reportes",
                        },
                    }).then((value) => {
                        switch (value) {
                            case "Reportes":
                                window.location.href = 'https:/Reportes/ResultadoExamenes';
                                break;
                            default:
                                dataTable.ajax.reload();
                        }
                    });
                }
                else { toastr.error(data.message); }
            },
        });
    }

    //Funcion llena los DropDownList Resultado de la tabla.
    function LlenarDropDownListResultado(parametros) {

        if (parametros.length > 0) {
            var listaResultado = ["Seleccione ↓"];
            for (var i = 0; i < parametros.length; i++) {
                listaResultado[i+1] = {},
                    listaResultado[i+1] = parametros[i].descripcion;
            };
        }
        else {
            var listaResultado = ["No Aplica..."];
        }
        return listaResultado;
    }
}