﻿var BeanSalidaTransporteCondicion = function (id_transporte_condicion, si_no) {
    this.Id = 0;
    this.Id_salida = 0;
    this.Id_transporte_condicion = id_transporte_condicion;
    this.Si_no = si_no;
}

var AudUni = function () {

    this.Init = init;
    var btn_valid_cp;
    var txt_cp;
    var txt_estado;
    var txt_municipio;
    var ddl_colonia;
    var txt_colonia;
    var div_colonia;
    var div_otra_razon;
    var div_prev_perdidas;
    var cod_prev_valido = false;
    var txt_cod_prev_perd;
    var div_pwd_per;

    var arrCondTran = [
        'Presenta Placas',
        'Licencia de conductor vigente',
        'Faros íntegros sin roturas',
        'Parabrisas íntegro sin rotura',
        'Cuenta con espejos laterales',
        'Libre de bultos o carga no identificada',
        'Piso íntegro sin daños',
        'Paredes de caga íntegras sin daños',
        'Techo de cada íntegro sin daños',
        'Puertas de caja íntegra con cerrojo funcional',
        'Pernos de caja soldados con tuerca de contra',
        'Presenta placas en caja parte posterior',
        'Limpia (sin basura en el interior)',
        'Íntegras sin desprendimientos o abultamientos',
        'Presenta todos los birlos',
        'Profundidad (huella de la llanta)',
        'Carga aprobada'
        ];
    var lstCondTran = [];

    function init() {
        btn_valid_cp = $('#btn_valid_cp');
        txt_cp = $('#ctl00_body_txt_cp');
        txt_estado = $('#ctl00_body_txt_estado');
        txt_municipio = $('#ctl00_body_txt_municipio');
        ddl_colonia = $('#ddl_colonia');
        txt_colonia = $('#ctl00_body_txt_colonia');
        div_colonia = $('#div_colonia');
        div_otra_razon = $('#div_otra_razon');
        div_prev_perdidas = $('#div_prev_perdidas');

        div_pwd_per = $('#div_pwd_per');

        $(div_colonia).dialog({
            autoOpen: false,
            //height: 300,
            //width: 480,
            modal: true,
            show: {
                effect: 'blind',
                duration: 1000
            },
            resizable: false
        });

        $(div_prev_perdidas).dialog({
            autoOpen: false,
            open: function (event, ui) {
                $(div_pwd_per).html('');
                $(div_pwd_per).html('<input style="width:0px; border: none; background-color:inherit;"><label for="txt_cod_prev_perd">C&oacute;digo de autorizaci&oacute;n</label><input type="password" id="txt_cod_prev_perd" autocomplete="off" readonly onfocus="this.removeAttribute(&quot;readonly&quot);"/>');
                txt_cod_prev_perd = $('#txt_cod_prev_perd');
                $(txt_cod_prev_perd).val('');
            },
            close: function (event, ui) {
                $(div_pwd_per).html('');
                $(txt_cod_prev_perd).val('');
            },
            //height: 300,
            //width: 480,
            modal: true,
            resizable: false,
            show: {
                effect: 'blind',
                duration: 1000
            },
            buttons: {
                'ok': function () {
                    cod_prev_valido = ('1234' == $(txt_cod_prev_perd).val());
                    $(this).dialog('close');
                    $('#ctl00_body_btnGuardar').trigger('click');
                }
            }
        });

        fillCondicionesTransporte();

        initEvents();
    }

    function initEvents() {
        $(btn_valid_cp).click(function () {
            validarCp();
            return false;
        });

        $(ddl_colonia).change(function () {
            $(txt_colonia).val($(this).val());
            $(div_colonia).dialog('close');
        });

        $('#ctl00_body_btnGuardar').button().click(function () {
            var IsValid = validarFrm();

            if (IsValid) {
                if (!cod_prev_valido) {
                    $(div_prev_perdidas).dialog('open');
                    return false;
                }
            }
            else
                return false;

            return IsValid == cod_prev_valido == true;
        });
    }

    function validarFrm() {
        var IsValid = true;

        condicionesTransporteSet();
        validaCondTransporte();

        $('.validator').each(function () {
            if ($(this).css('visibility') == 'visible') {
                $('html,body').animate({
                    scrollTop: $(this).offset().top
                }, 2000);
                IsValid = false;
                return false;
            }
        });

        //        if (IsValid && cod_prev_valido) {
        //            $('#ctl00_body_hf_click_save').val('1');
        //            $(this).hide();
        //        }

        return IsValid;
    }

    function validarCp() {
        $(ddl_colonia).html('');
        $(txt_colonia).val('');
        $.ajax({
            type: 'GET',
            url: 'https://api-codigos-postales.herokuapp.com/v2/codigo_postal/' + $(txt_cp).val(),
            complete: function () {
                $('#div-info-codigo').removeClass('ajaxLoading');
            },
            success: function (data) {
                $(txt_estado).val(data.estado);
                $(txt_municipio).val(data.municipio);
                if (data.colonias.length > 1) {
                    var arrCol = [];
                    for (var i = 0; i < data.colonias.length; i++) {
                        var col = {
                            datatext: data.colonias[i],
                            datavalue: data.colonias[i]
                        }
                        arrCol.push(col);
                    }
                    Common.fillSelect('ddl_colonia', arrCol);
                    $(div_colonia).dialog('open');
                    $(txt_colonia).val(arrCol[0].datavalue);
                } else {
                    $(txt_colonia).val(data.colonias[0]);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var oErrorMessage = new ErrorMessage();
                oErrorMessage.SetError(jqXHR.responseText);
                oErrorMessage.Init();
            }
        });
    }

    function fillCondicionesTransporte() {
        $('#tbody_condiciones').html('');
        var tr;
        var td;
        var ind = 1;
        for (var itemCT in arrCondTran) {
            tr = '<tr id="condTr_' + ind + '">';
            td = '<td>';
            td += arrCondTran[itemCT];
            td += '</td>';
            tr += td;
            td = '<td><input name="name_' + ind + '" type="radio" value="1" /></td>';
            tr += td;
            td = '<td><input name="name_' + ind + '" type="radio" value="0" /></td>';
            tr += td;
            $('#tbody_condiciones').append(tr);
            ind++;
        }
        $('#condTr_' + arrCondTran.length).children('td').each(function () {
            $(this).children('input').each(function () {
                $(this).click(function () {
                    var valor = $(this).val();
                    if (valor == 1) {
                        $(div_otra_razon).addClass('hidden');
                    }
                    else {
                        $(div_otra_razon).removeClass('hidden');
                    }
                });
            });
        });
    }

    //Condiciones del transporte
    function condicionesTransporteSet() {
        lstCondTran = [];
        $('#tbody_condiciones').children('tr').each(function () {
            var id = $(this).attr('id').split('_')[1] * 1;
            var val = $('input[name="name_' + id + '"]:checked', '#tbody_condiciones').val();
            if (val != undefined) {
                val = val == 1 ? true : false;
                var o = new BeanSalidaTransporteCondicion(id, val);
                lstCondTran.push(o);
            }
        });
        $('#ctl00_body_hf_condiciones_transporte').val(JSON.stringify(lstCondTran));
    }

    //valida condiciones transporte
    function validaCondTransporte() {
        $('#rfv_condiciones_transporte').css('visibility', lstCondTran.length == arrCondTran.length ? 'hidden' : 'visible');
    }
}

var master = new webApp.Master;
var pag = new AudUni();
master.Init(pag);