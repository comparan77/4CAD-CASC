﻿var lstDoc = [];
var lstEntComp = [];
var lstEntTran = [];
var lstDocumento = [];

var idEntradaTransporte = 0;
var BeanEntradaDocumento = function (id_documento, Pdocumento, referencia) {
    this.Id = 0;
    this.Id_entrada = 0;
    this.Id_documento = id_documento;
    this.Referencia = referencia;
    this.PDocumento = Pdocumento;
}
var BeanDocumento = function (id_documento, nombre, mascara) {
    this.Id = id_documento;
    this.Nombre = nombre;
    this.Mascara = mascara;
    this.IsActive = true;
}
var BeanEntradaCompartido = function (id_usuario, referencia) {
    this.Id = 0;
    this.Id_entrada = null;
    this.Id_usuario = id_usuario;
    this.Folio = '';
    this.Referencia = referencia;
    this.Capturada = false;
    this.IsActive = true;
}

var BeanEntradaTransporte = function (id, transporteLinea, idTransporteTipo, transporteTipo, placa, caja, caja1, caja2) {
    this.Id = id;
    this.Id_entrada = 0;
    this.Transporte_linea = transporteLinea;
    this.Id_transporte_tipo = idTransporteTipo;
    this.Placa = placa;
    this.Caja = caja;
    this.Caja1 = caja1;
    this.Caja2 = caja2;
    this.Transporte_tipo = transporteTipo;
}

var MngArribo = function () {

    this.Init = init;

    function init() {

        //Llena select de documentos
        fillLstDocumento();

        //Llena select de clientes
        fillLstCliente();

        //Cambio de bodega
        $('#ctl00_body_ddlBodega').change(function () {
            $('#ctl00_body_up_bodega').addClass('ajaxLoading');
        });

        $('#ctl00_body_up_bodega').panelReady(function () {
            $('#ctl00_body_up_bodega').removeClass('ajaxLoading');
        });

        //Es compartida pendiente
        if (verificaCompartidaPendiente() != 1) {
            //Cambio de clientes
            $('#ctl00_body_ddlCliente').change(function () {
                ddlClienteChange($(this).val());
            }).trigger('change');
        }

        //Cambio tipo de documento
        $('#ddlDocumento').change(function () {
            validaTipoDocumento();
        }).trigger('change');

        $('#ctl00_body_btn_save').button();
        $('#ctl00_body_btn_buscar').button();
        $('#ctl00_body_txt_referencia, #txt_pedimento_compartido').mask('99-9999-9999999');
        $('#ctl00_body_txt_hora_llegada, #ctl00_body_txt_hora_descarga').scroller({
            preset: 'time',
            theme: 'default',
            display: 'modal',
            mode: 'clickpick',
            timeFormat: 'HH:ii:ss'
        });
        $('#btn_add_documento').click(function () {
            addDocumento();
            return false;
        });

        validaTipoTransporte();
        $('#ctl00_body_ddlTipo_Transporte').change(function () {
            validaTipoTransporte();
        }).trigger('change');

        $('#btn_add_tipoTransporte').click(function () {
            addTransporte();
            verificaTranportes();
            return false;
        });

        $('#btn_show_cantidadesProblema').button().click(function () {
            $(this).hide();
            $('#cantidadesProblema').show('slow');
            return false;
        });

        $('.rbTipoEntrada').buttonset().children('input:radio').each(function () {
            verificaTipoEntrada($(this));
            $(this).change(function () {
                verificaTipoEntrada($(this));
            });
        });

        $('#ctl00_body_btn_save').click(function () {
            var IsValid = true;
            //valida transportes
            verificaTranportes();
            verificaDocumentosRequeridos();

            $('.validator').each(function () {
                if ($(this).css('visibility') == 'visible') {
                    $('html,body').animate({
                        scrollTop: $(this).offset().top
                    }, 2000);
                    IsValid = false;
                    return false;
                }
            });

            //return IsValid;

            if (IsValid) {
                var confirm = $('#hf-confirmado').val() * 1;
                if (confirm == 0) {
                    var mensaje = '';
                    mensaje = '<b>Tipo:</b> ' + $('#spnTipoEntrada').html();
                    if ($('#ctl00_body_chk_ultima').is(':checked') == true) {
                        mensaje += ', <b>Última entrada.</b>';
                    }
                    mensaje += '<br />';
                    mensaje += '<ul>';
                    $(lstEntTran).each(function (i, itemET) {
                        mensaje += '<li>';
                        mensaje += itemET.Transporte_linea + ', ' + itemET.Transporte_tipo + ', Placa: ' + itemET.Placa + ', Caja: ' + itemET.Caja + ', Cont1: ' + itemET.Caja1 + ', Cont2: ' + itemET.Caja2;
                        mensaje += '</li>';
                    });
                    mensaje += '</ul>';
                    mensaje += '<hr />';
                    mensaje += '<b>Compartida:</b> ' + (lstEntComp.length > 0 ? 'Sí' : 'No');
                    //Confirmarción de piezas y bultos
                    mensaje += '<hr />';
                    mensaje += '<h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-header-active ui-state-active ui-corner-top">Confirmar cantidades</h3>';
                    mensaje += '<div class="divForm">';
                    mensaje += '<div><label>Bultos Recibidos:</label><input type="text" class="txtNumber" id="txt_no_bulto_recibido_confirm" /></div>';
                    mensaje += '<div><label>Piezas Recibidas:</label><input type="text" class="txtNumber" id="txt_no_pieza_recibida_confirm" /></div>';
                    mensaje += '</div>';
                    $('#spn-aviso-registro').html(mensaje);
                    $("#dialog-confirm").dialog('open');


                    $('.confirmValue').each(function () {
                        $(this).addClass('blurry-text');
                    });

                    IsValid = false;
                }
                else {
                    $(this).hide();
                    IsValid = true;
                }
            }
            return IsValid;
        });

        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: 380,
            width: 450,
            modal: true,
            close: function () {
                $('.confirmValue').each(function () {
                    $(this).removeClass('blurry-text');
                });
            },
            buttons: {
                "Guardar Entrada": function () {
                    $(this).dialog("close");

                    if ($('#ctl00_body_txt_no_bulto_recibido').val() == $('#txt_no_bulto_recibido_confirm').val() && $('#ctl00_body_txt_no_pieza_recibida').val() == $('#txt_no_pieza_recibida_confirm').val()) {
                        $('#hf-confirmado').val(1);
                        var clickButton = document.getElementById('ctl00_body_btn_save');
                        clickButton.click();
                    }
                    else {
                        $('.confirmValue').each(function () {
                            var attr = $(this).attr('disabled');
                            if (typeof attr == typeof undefined) {
                                $(this).val('');
                            }
                        });
                        alert('Las cantidades proporcionadas no coinciden, favor de verficarlas.');
                        $('#ctl00_body_txt_no_pieza_recibida').val($('#ctl00_body_hf_sum_piezas_partidas').val());
                    }

                },
                Cancel: function () {
                    $(this).dialog("close");
                    $('.confirmValue').each(function () {
                        $(this).removeClass('blurry-text');
                    });
                }
            }
        });

        $('#ctl00_body_up_partidas').panelReady(function () {
            $('#ctl00_body_txt_no_pieza_recibida').val($('#ctl00_body_hf_sum_piezas_partidas').val());
        });

    } // init <<fin>>

    function verificaCompartidaPendiente() {
        var CompPend = $('#ctl00_body_hf_CompPendiente').val() * 1;
        var pnl_compartidos = $('#ctl00_body_pnl_compartidos');
        var pnlBusqueda = $('#ctl00_body_pnl_busqueda');
        var pnl_infArribo = $('#pnl_infArribo');
        pnl_compartidos.hide();
        var idusuario = $('#ctl00_body_hf_id_usuario').val();

        $('.pedComp').each(function () {
            var oEntComp = new BeanEntradaCompartido(idusuario, $(this).html());
            lstEntComp.push(oEntComp);
        });

        if (CompPend == 1) {

            $('.ulCompartida').children('li').each(function () {
                $(this).children('input').button();
            });

            pnl_compartidos.show();
            pnlBusqueda.hide();
            pnl_infArribo.hide();
        }
        return CompPend;
    }

    function fillLstCliente() {

        var jsonCliente = $('#ctl00_body_hf_clientes').val();
        jsonCliente = $.parseJSON(jsonCliente);

        $(jsonCliente).each(function (i, obj) {
            var id_cliente = $('#ctl00_body_hf_id_cliente').val();
            $('#ctl00_body_ddlCliente').append('<option ' + (id_cliente == obj.Id ? ' selected ' : '') + ' documento="' + obj.Documento + '" fondeo="' + obj.EsFondeo + '" razon="' + obj.Razon + '" mask="' + obj.Mascara + '" value="' + obj.Id + '">' + obj.Nombre + '</option>');
        });
    }

    function fillLstDocumento() {

        var jsonDocumento = $('#ctl00_body_hf_Documentos').val();
        jsonDocumento = $.parseJSON(jsonDocumento);

        $(jsonDocumento).each(function (i, obj) {
            var oBD = new BeanDocumento(obj.Id, obj.Nombre, obj.Mascara);
            lstDocumento.push(oBD);
        });
    }

    function addDocumento() {
        var idDocumento = $('#ddlDocumento').val();
        var tipoDocumento = $('#ddlDocumento>option:selected').text();
        var referencia = $('#txt_documento').val();
        var hf_entradaDocumento = $('#ctl00_body_hf_entradaDocumento');

        if (referencia.length < 1)
            return false;

        var oDoc = new BeanDocumento(idDocumento, tipoDocumento);
        var oEntDoc = new BeanEntradaDocumento(idDocumento, oDoc, referencia);
        var arrDocEx = $.grep(lstDoc, function (obj) {
            return obj.Id_documento == idDocumento;
        });
        if (arrDocEx.length == 0)
            lstDoc.push(oEntDoc);
        var tr = '';
        $('#tbody_documentos').html('');

        for (var iDoc in lstDoc) {
            tr = '<tr id="' + lstDoc[iDoc].Id_documento + '">';
            var td = '<td>' + lstDoc[iDoc].PDocumento.Nombre + '</td>';
            var arrRefs = [];
            var tdHtmlRef = '';
            arrRefs = lstDoc[iDoc].Referencia.split(',');
            $.each(arrRefs, function (i, j) {
                tdHtmlRef += arrRefs[i] + '<br />';
            });
            td += '<td>' + tdHtmlRef + '</td>';
            td += '<td><button class="rem_documento" id="btn_rem_documento"><span class="ui-icon ui-icon-trash"></button></td>';
            tr += td;
            tr += '</tr>';
            $('#tbody_documentos').append(tr);
        }
        $(hf_entradaDocumento).val(JSON.stringify(lstDoc));
        $('.rem_documento').each(function () {
            $(this).click(function () {
                //alert($(this).parent().parent().attr('id'));
                var Id_Documento = $(this).parent().parent().attr('id');
                lstDoc = $.grep(lstDoc, function (obj) {
                    return obj.Id_documento != Id_Documento;
                });
                $(this).parent().parent().remove();
                $(hf_entradaDocumento).val(JSON.stringify(lstDoc));
                return false;
            });
        });
    } // fin addDocumento

    function addCompartido() {
        var idusuario = $('#ctl00_body_hf_id_usuario').val();
        var referencia = $('#txt_pedimento_compartido').val();
        var hf_arribo_compartido = $('#ctl00_body_hf_arribo_compartido');
        var pedimento = $('#ctl00_body_txt_referencia').val();
        var txt_doc_req = $('#ctl00_body_txt_doc_req').val();

        if (referencia == pedimento || referencia == txt_doc_req) {
            alert('El pedimento proporcionado, es el principal de este arribo');
            return false;
        }

        var oEntComp = new BeanEntradaCompartido(idusuario, referencia);
        var arrEntComp = $.grep(lstEntComp, function (obj) {
            return obj.Referencia == referencia;
        });
        if (arrEntComp.length == 0)
            lstEntComp.push(oEntComp);
        var tr = '';
        $('#tbody_pedimentos').html('');
        for (var iEntComp in lstEntComp) {
            tr = '<tr id="' + lstEntComp[iEntComp].Referencia + '">';
            var td = '<td>' + lstEntComp[iEntComp].Referencia + '</td>';
            td += '<td><button class="rem_pedimento" id="btn_rem_pedimento"><span class="ui-icon ui-icon-trash"></button></td>';
            tr += td;
            tr += '</tr>';
            $('#tbody_pedimentos').append(tr);
        }
        $(hf_arribo_compartido).val(JSON.stringify(lstEntComp));
        $('.rem_pedimento').each(function () {
            $(this).click(function () {
                var refer = $(this).parent().parent().attr('id');
                lstEntComp = $.grep(lstEntComp, function (obj) {
                    return obj.Referencia != refer;
                });
                $(this).parent().parent().remove();
                $(hf_arribo_compartido).val(JSON.stringify(lstEntComp));
                return false;
            });
        });
    } // fin addCompartido

    // valida Tipo de documento
    function validaTipoDocumento() {
        $('#ddlDocumento option:selected').each(function () {
            var Mascara = $(this).attr('mask');
            var txt_documento = $('#txt_documento');
            txt_documento.unmask();
            if (Mascara.length > 0)
                txt_documento.mask(Mascara);
        });
    }

    // valida cliente
    function validaCliente(data) {
        $('#ctl00_body_ddlCliente option:selected').each(function () {
            $('#ctl00_body_hf_id_cliente').val($(this).attr('value'));
            $('#ctl00_body_hf_cliente_nombre').val($('#ctl00_body_ddlCliente option:selected').attr('razon'));
            var EsFondeo = $(this).attr('fondeo').toUpperCase() == "TRUE";
            var DocumentoReq = $(this).attr('documento');
            //la máscara va en función del documento principal definido en el catálogo
            var objDocReq;
            var Mascara;
            objDocReq = $.grep(data, function (obj) {
                return obj.Es_principal == 1;
            });
            if (objDocReq.length > 0) {
                objDocReq = $.grep(lstDocumento, function (obj) {
                    return obj.Id == objDocReq[0].Id_documento;
                });
                DocumentoReq = objDocReq[0].Nombre;
                Mascara = objDocReq[0].Mascara;
            }
            //fin

            var pnlBusqueda = $('#ctl00_body_pnl_busqueda');
            var pnlInfoArribo = $('#ctl00_body_pnl_infoArribo');
            var div_doc_requerido = $('#div_doc_requerido');

            var referenciaAux = div_doc_requerido.children('input').val();

            pnlBusqueda.hide();
            pnlInfoArribo.hide();
            div_doc_requerido.children('label').html('');
            div_doc_requerido.children('input').unmask();
            div_doc_requerido.children('span').html('Es requerido');

            $('#txt_documento').val('');

            $('#tbody_documentos').html('');
            lstDoc = [];

            $('#tbody_pedimentos').html('');
            lstEntComp = [];
            $('#txt_pedimento_compartido').val('');

            // Para pedimentos compartidos
            $('#btn_add_compartido').unbind('click').click(function () {
                validaCompartido(EsFondeo);
                return false;
            });

            $('#ctl00_body_txt_origen').removeAttr('disabled');

            // Limpia el contenido de los documentos requeridos
            $('#docsRequeridos').html('');

            if (EsFondeo) {
                pnlBusqueda.show();
                fillDdlDocumento('Pedimento');
                div_doc_requerido.hide();
                $('#ctl00_body_txt_origen').attr('disabled', 'disabled');
                $('#ctl00_body_txt_no_pieza_declarada').attr('disabled', 'disabled');
                $('#txt_documento').val($('#ctl00_body_hf_facturasAvon').val());
                addDocumento();
                if ($('#ctl00_body_hf_fondeoValido').val() == '1') {
                    pnlBusqueda.hide();
                    pnlInfoArribo.show();
                    div_doc_requerido.show();
                    div_doc_requerido.children('label').html('Pedimento');
                }
                if ($('#ctl00_body_hf_click_Compartida').val() == '1') {
                    pnlBusqueda.hide();
                    pnlInfoArribo.show();
                    div_doc_requerido.show();
                    div_doc_requerido.children('label').html('Pedimento');
                }
            }
            else {
                $('#ctl00_body_txt_no_pieza_declarada').removeAttr('disabled');

                if ($('#ctl00_body_hf_click_Compartida').val() == '1') {
                    $('#ctl00_body_txt_origen').attr('disabled', 'disabled');
                }

                pnlInfoArribo.show();
                div_doc_requerido.show();
                var pedimentoValidado = false;
                $('#div_partidas').hide();
                if (data.length > 0) {
                    //                    div_doc_requerido.children('label').html(DocumentoReq);
                    //                    if (Mascara.length > 0)
                    //                        div_doc_requerido.children('input').mask(Mascara);
                    //                    div_doc_requerido.children('span').html('Es necesario proporcionar ' + DocumentoReq);
                    //                    fillDdlDocumento(data, DocumentoReq);                   
                    $.each(data, function (i, oDocReq) {
                        objDocReq = $.grep(lstDocumento, function (obj) {
                            return obj.Id == oDocReq.Id_documento;
                        });
                        if (!pedimentoValidado) {
                            if (oDocReq.Id_documento == 1) {
                                pedimentoValidado = true;
                                $('#div_partidas').show();
                            }
                        }
                        var divDocReq = '<div id="divReq_' + objDocReq[0].Id + '" style="margin-bottom: 5px"><label>' + objDocReq[0].Nombre + '</label><input es_principal="' + oDocReq.Es_principal + '" type="text" id="txt_' + objDocReq[0].Nombre + '" /><span class="validator" style="color:Red;visibility:hidden;"></span></div>';
                        $('#docsRequeridos').append(divDocReq);
                        if (objDocReq[0].Mascara.length > 0)
                            $('#txt_' + objDocReq[0].Nombre).mask(objDocReq[0].Mascara);
                        $('#txt_' + objDocReq[0].Nombre).next().html('Es necesario capturar ' + objDocReq[0].Nombre);
                    });
                    fillDdlDocumento(data, DocumentoReq);
                }
                else {
                    div_doc_requerido.children('input').val('NA');
                    fillDdlDocumento(data, '');
                    div_doc_requerido.hide();
                }
            }

        });
    }

    function fillDdlDocumento(data, hideOption) {
        var ddlDocumento = $('#ddlDocumento');
        ddlDocumento.html('');
        var requerido = false;
        var objDoc;
        $(lstDocumento).each(function (i, obj) {
            objDoc = $.grep(data, function (o) {
                return o.Id_documento == obj.Id;
            });
            if (objDoc.length == 0) {
                ddlDocumento.append('<option value="' + obj.Id + '" mask="' + obj.Mascara + '">' + obj.Nombre + '</option>');
            }
        });
    }

    // valida tipo transporte <<ini>>
    function validaTipoTransporte() {
        $('#ctl00_body_ddlTipo_Transporte option:selected').each(function () {
            txt_dato_transRequerido($('#txt_placa'), $(this).attr('placa'));
            txt_dato_transRequerido($('#txt_caja'), $(this).attr('caja'));
            txt_dato_transRequerido($('#txt_caja1'), $(this).attr('caja1'));
            txt_dato_transRequerido($('#txt_caja2'), $(this).attr('caja2'));
        });
    }

    function txt_dato_transRequerido(txt, requerido) {
        $(txt).attr('readonly', 'readonly').val('N.A.');
        if (requerido == 'True') {
            $(txt).removeAttr('readonly');
            $(txt).val('');
        }
    }
    // valida tipo transporte <<fin>>

    function addTransporte() {
        var hf_entradaTransporte = $('#ctl00_body_hf_entradaTransporte');
        var transporteLinea = $('#txt_linea').val().toUpperCase().trim();
        var idTransporteTipo = $('#ctl00_body_ddlTipo_Transporte').val();
        var transporteTipo = $("#ctl00_body_ddlTipo_Transporte option:selected").text();
        var placa = $('#txt_placa').val().toUpperCase().trim();
        var caja = $('#txt_caja').val().toUpperCase().trim();
        var caja1 = $('#txt_caja1').val().toUpperCase().trim();
        var caja2 = $('#txt_caja2').val().toUpperCase().trim();

        if (!TransporteValido(transporteLinea, placa, caja, caja1, caja2))
            return false;

        idEntradaTransporte++;
        var oEntTran = new BeanEntradaTransporte(idEntradaTransporte, transporteLinea, idTransporteTipo, transporteTipo, placa, caja, caja1, caja2);

        var arrEntTran = $.grep(lstEntTran, function (obj) {
            return (obj.Transporte_linea == transporteLinea
                && obj.Id_transporte_tipo == idTransporteTipo
                && obj.Placa == placa
                && obj.Caja == caja
                && obj.Caja1 == caja1
                && obj.Caja2 == caja2);
        });
        if (arrEntTran.length == 0)
            lstEntTran.push(oEntTran);

        var tr = '';
        $('#tbody_transporte').html('');
        for (var iT in lstEntTran) {
            tr = '<tr id="' + lstEntTran[iT].Id + '">';
            var td = '<td>' + lstEntTran[iT].Transporte_linea + '</td>';
            td += '<td>' + lstEntTran[iT].Transporte_tipo + '</td>';
            td += '<td>' + lstEntTran[iT].Placa + '</td>';
            td += '<td>' + lstEntTran[iT].Caja + '</td>';
            td += '<td>' + lstEntTran[iT].Caja1 + '</td>';
            td += '<td>' + lstEntTran[iT].Caja2 + '</td>';
            td += '<td><button class="rem_transporte"><span class="ui-icon ui-icon-trash"></button></td>';
            tr += td;
            tr += '</tr>';
            $('#tbody_transporte').append(tr);
        }
        $(hf_entradaTransporte).val(JSON.stringify(lstEntTran));
        $('.rem_transporte').each(function () {
            $(this).click(function () {
                //alert($(this).parent().parent().attr('id'));
                var Id_EntTran = $(this).parent().parent().attr('id');
                lstEntTran = $.grep(lstEntTran, function (obj) {
                    return obj.Id != Id_EntTran;
                });
                $(this).parent().parent().remove();
                $(hf_entradaTransporte).val(JSON.stringify(lstEntTran));
                return false;
            });
        });
    } // fin addTransporte

    function verificaTipoEntrada(radiobtn) {
        $('#divParcialidad').addClass('hidden');
        $('#spnNoEntradaParcial').html('');
        if ($(radiobtn).is(':checked')) {
            switch ($(radiobtn).val()) {
                case 'rb_parcial':
                    $('#spnTipoEntrada').html('Se ha seleccionado entrada Parcial');
                    $('#spnNoEntradaParcial').html('No. Entrada: ' + $('#ctl00_body_hf_no_entrada_parcial').val());
                    $('#divParcialidad').removeClass('hidden');
                    break;
                case 'rb_unica':
                    $('#spnTipoEntrada').html('Se ha seleccionado entrada Única');
                    break;
            }
        }
    }

    function verificaTranportes() {
        $('#rfv_entradaTransporte').css('visibility', lstEntTran.length == 0 ? 'visible' : 'hidden');
    }

    function verificaDocumentosRequeridos() {
        $('#docsRequeridos div input').each(function () {
            $(this).next().css('visibility', $(this).val().length == 0 ? 'visible' : 'hidden');
            if ($(this).attr('es_principal') == 'true')
                $('#ctl00_body_hf_referencia').val($(this).val());
            else {
                var idDocumento = $(this).parent().attr('id').split('_')[1];
                var oDoc = new BeanDocumento(idDocumento, $(this).prev().html());
                var oEntDoc = new BeanEntradaDocumento(idDocumento, oDoc, $(this).val());
                var arrDocEx = $.grep(lstDoc, function (obj) {
                    return obj.Id_documento == idDocumento;
                });
                debugger;
                if (arrDocEx.length == 0) {
                    lstDoc.push(oEntDoc);
                    $('#ctl00_body_hf_entradaDocumento').val(JSON.stringify(lstDoc));
                }
            }
        });
    }

    function TransporteValido(linea, placa, caja, caja1, caja2) {
        var strDatos = ['linea', 'placa', 'caja', 'caja1', 'caja2'];
        for (var dato in strDatos) {
            var input = $('#txt_' + strDatos[dato]);
            var info = $(input).val().toUpperCase().trim();
            if (info.length == 0) {
                $(info).focus();
                alert('Es necesario capturar la: ' + strDatos[dato]);
                return false;
            }
        }
        return true;
    }

    function validaCompartido(EsFondeo) {
        var referencia = $('#txt_pedimento_compartido').val();
        if (referencia.length < 1)
            return false;

        if (EsFondeo) {
            $.ajax({
                type: 'GET',
                url: "/handlers/Operation.ashx",
                //dataType: "jsonp",
                data: {
                    op: 'arribo',
                    ref: referencia
                },
                success: function (data) {
                    if (data.toString() == 'true')
                        addCompartido();
                    else {
                        var oErrorMessage = new ErrorMessage();
                        oErrorMessage.SetError(data);
                        oErrorMessage.Init();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var oErrorMessage = new ErrorMessage();
                    oErrorMessage.SetError(jqXHR.responseText);
                    oErrorMessage.Init();
                }
            });
        }
        else
            addCompartido();
    }

    function ddlClienteChange(id_cliente) {
        //        $('#ctl00_body_ddlCliente').change(function () {
        //            validaCliente();
        //            $('#ddlDocumento').trigger('change');
        //            $('#ctl00_body_hf_fondeoValido').val('')
        //        }).trigger('change');

        $.ajax({
            type: 'GET',
            url: "/handlers/Catalog.ashx?catalogo=cliente_documento&opt=getList&key=" + id_cliente,
            //dataType: "jsonp",
            //            data: {
            //                op: 'arribo',
            //                ref: referencia
            //            },
            success: function (data) {
                validaCliente(data);
                $('#ddlDocumento').trigger('change');
                $('#ctl00_body_hf_fondeoValido').val('');
                //$('#ctl00_body_ddlCliente').trigger('change');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var oErrorMessage = new ErrorMessage();
                oErrorMessage.SetError(jqXHR.responseText);
                oErrorMessage.Init();
            }
        });

    }
}

var master = new webApp.Master;
var pag = new MngArribo();
master.Init(pag);