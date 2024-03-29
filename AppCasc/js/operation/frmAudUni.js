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
    var up_prev_pred_user;
    var ddlBodega;
    var ddl_autoriza;
    var hf_usr_prv_perd;
    var hf_cond_trans;
    var upDatosVehiculo;
    var upTipoTransporte;
    var ddlTipo_Transporte;
    var ddlTransporte;
    var rb_noaprobada;
    var rb_aprobada;
    var btn_consultar;
    var up_consulta;

    var arrCondTran = [];
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
        up_prev_pred_user = $('#ctl00_body_up_prev_pred_user');
        ddlBodega = $('#ctl00_body_ddlBodega');
        ddl_autoriza = $('#ddl_autoriza');
        hf_usr_prv_perd = $('#ctl00_body_hf_usr_prv_perd');
        hf_cond_trans = $('#ctl00_body_hf_cond_trans');
        upDatosVehiculo = $('#ctl00_body_upDatosVehiculo');
        upTipoTransporte = $('#ctl00_body_upTipoTransporte');
        ddlTransporte = $('#ctl00_body_ddlTransporte');
        rb_noaprobada = $('#ctl00_body_rb_noaprobada');
        rb_aprobada = $('#ctl00_body_rb_aprobada');
        btn_consultar = $('#ctl00_body_btn_consultar');
        up_consulta = $('#ctl00_body_up_consulta');

        $('#ctl00_body_txt_fecha_ini, #ctl00_body_txt_fecha_fin').datepicker({
            'dateFormat': 'dd/mm/yy'
        });

        $('#tabs').tabs();

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
                //<input style="width:0px; border: none; background-color:inherit;">
                $(div_pwd_per).html('<label for="txt_cod_prev_perd">C&oacute;digo de autorizaci&oacute;n</label><input type="password" id="txt_cod_prev_perd" autocomplete="off" readonly onfocus="this.removeAttribute(&quot;readonly&quot);"/>');
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
                    validaPersonalPrev($(ddl_autoriza).val(), $(txt_cod_prev_perd).val());
                }
            }
        });

        //fillCondicionesTransporte();

        initEvents();
    }

    function initEvents() {

        $(btn_consultar).button().click(function () {
            consultar_reg();
        });

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

        $(up_consulta).panelReady(function () {
            webApp.Master.loading(false);
        });

        $(up_prev_pred_user).panelReady(function () {
            ddl_autoriza = $('#ddl_autoriza');
            fillPersonalPrev();
            $(ddlBodega).change(function () {
                fillPersonalPrev();
            });
        });

        $(ddlTransporte).change(function () {
            webApp.Master.loading(true);
        });

        $(upTipoTransporte).panelReady(function () {
            //webApp.Master.loading();
            ddlTipo_Transporte = $('#ctl00_body_ddlTipo_Transporte');
            $(ddlTipo_Transporte).change(function () {
                webApp.Master.loading(true);
            });
            webApp.Master.loading(false);
        });

        $(upDatosVehiculo).panelReady(function () {
            hf_cond_trans = $('#ctl00_body_hf_cond_trans');
            fillCondicionesTransporte();
            webApp.Master.loading(false);
        });

        $(rb_noaprobada).click(function () {
            $('#div_rechazo').removeClass('hidden');
        });

        $(rb_aprobada).click(function () {
            $('#div_rechazo').addClass('hidden');
        });
    }

    function consultar_reg() {
        webApp.Master.loading(true);
    }

    function validaPersonalPrev(clave, contrasenia) {
        cod_prev_valido = false;
        $.ajax({
            type: 'POST',
            url: '/handlers/CAEApp.ashx?op=usuario&&opt=UsuarioValido',
            data: JSON.stringify({
                Id: 0,
                Nombre: '',
                Clave: clave,
                Email: '',
                Contrasenia: contrasenia,
                Id_bodega: 0,
                Id_rol: 0,
                IsActive: 1
            }),
            complete: function () {
                $('#div-info-codigo').removeClass('ajaxLoading');
            },
            success: function (data) {
                switch (typeof (data)) {
                    case 'string':
                        alert('Las credenciales proporcionadas no son correctas');
                        break;
                    case 'object':
                        cod_prev_valido = true;
                        $(div_prev_perdidas).dialog('close');
                        $('#ctl00_body_btnGuardar').trigger('click');
                        break;
                    default:
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var oErrorMessage = new ErrorMessage();
                oErrorMessage.SetError(jqXHR.responseText);
                oErrorMessage.Init();
            }
        });
    }

    function fillPersonalPrev() {
        $(ddl_autoriza).html('');
        var arrAut = JSON.parse($(hf_usr_prv_perd).val());
        var arrCol = [];
        for (var aut in arrAut) {
            var item = arrAut[aut];
            var col = {
                datatext: item.Nombre,
                datavalue: item.Clave
            }
            arrCol.push(col);
        }
        Common.fillSelect('ddl_autoriza', arrCol);
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
        webApp.Master.loading(true);
        $.ajax({
            type: 'GET',
            url: 'https://api-codigos-postales.herokuapp.com/v2/codigo_postal/' + $(txt_cp).val(),
            complete: function () {
                webApp.Master.loading(false);
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
        var objCondTrans;
        arrCondTran = JSON.parse($(hf_cond_trans).val());
        //console.log($(hf_cond_trans).val());
        //alert($(hf_cond_trans).val());

        $('#ctl00_body_hf_num_cond').val(arrCondTran.length);
        var idCat = 0;
        var rowSpan = 0;
        var arrCatg = [];

        for (var itemCT in arrCondTran) {
            objCondTrans = arrCondTran[itemCT];
            tr = '<tr id="condTr_' + ind + '">';
            if (idCat != objCondTrans.PTransCondCat.Id) {
                arrCatg = arrCondTran.filter(function (obj) {
                    return obj.PTransCondCat.Id == objCondTrans.PTransCondCat.Id;
                });
                td = '<td rowspan="' + arrCatg.length + '" id="tdCondCat_' + idCat + '">' + objCondTrans.PTransCondCat.Nombre + '</td>';
                td += '<td>';
                idCat = objCondTrans.PTransCondCat.Id;
            } else {
                td = '<td>';
                rowSpan++;
            }

            td += objCondTrans.Nombre;
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