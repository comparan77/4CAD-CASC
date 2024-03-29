﻿var beanCliente_mercancia = function (id, id_cliente_grupo, codigo, nombre, clase, negocio, unidad) {

    this.Id = id;
    this.Id_cliente_grupo = id_cliente_grupo;
    this.Codigo = codigo;
    this.Nombre = nombre;
    this.Clase = clase;
    this.Negocio = negocio;
    this.Valor_unitario = 0;
    this.Unidad = unidad;
    this.Presentacion_x_bulto = 0;
    this.Bultos_x_tarima = 0;
    this.IsActive = true;
}

var ctrlClienteMercancia = function () {
    this.Init = init;
    this.OpenFrm = openFrm;
    this.OpenFrmUdt = openFrmUdt;
    this.FindByCode = findByCode;

    function init() {
    }

    function initControls(page, action) {
        dialog = $("#ctrlClienteMercancia").dialog({
            autoOpen: false,
            //height: 300,
            width: 480,
            modal: true,
            resizable: false,
            buttons: {
                "Guardar Mercancía": function () {
                    if (validaRequeridos())
                        MngMercancia(page, action);
                },
                "Cancelar": function () {
                    $(this).dialog('close');
                }
            }
        });
    }

    function openFrm(cod, page) {
        initControls(page, 'ist');
        $('#txt_codigo').val(cod);
        $('#h_id_cliente_mercancia').val('0');
        $("#ctrlClienteMercancia").dialog('open');

        $('#txt_clase').combobox({ maxLength: 3 });
        $('#txt_negocio').combobox({ maxLength: 3 });
        $('#txt_unidad').combobox({ maxLength: 2 });
    }

    function openFrmUdt(obj, page) {
        initControls(page, 'udt');
        $('#txt_codigo').val(obj.Codigo);
        $('#txt_nombre').val(obj.Nombre);
        $('#h_id_cliente_mercancia').val(obj.Id);
        $('#txt_negocio').val(obj.Negocio);
        $("#ctrlClienteMercancia").dialog('open');

        $('#txt_clase').combobox({ maxLength: 3 });
        $('#txt_negocio').combobox({ maxLength: 3 });
        $('#txt_unidad').combobox({ maxLength: 2 });
    }

    function validaRequeridos() {
        var valid = true;
        $('.requeridoCM').each(function () {
            if ($(this).val() == '' || $(this).val() == null) {
                alert('Es necesario capturar: ' + $(this).prev().html());
                $(this).focus();
                valid = false;
                return false;
            }

        });
        return valid;
    }

    function findByCode(page, codigo, id_cliente_grupo) {

        var oCM = new beanCliente_mercancia(0, id_cliente_grupo, codigo, '', '', '', '');

        $.ajax({
            type: 'POST',
            url: '/handlers/Catalog.ashx?catalogo=cliente_mercancia&opt=findByCode',
            //dataType: "jsonp",
            data: JSON.stringify(oCM),
            complete: function () {

            },
            success: function (data) {
                page.Recall(data, page.CtrlCM);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var oErrorMessage = new ErrorMessage();
                oErrorMessage.SetError(jqXHR.responseText);
                oErrorMessage.Init();
            }
        });
    }

    function getCliente_mercancia() {
        return new beanCliente_mercancia($('#h_id_cliente_mercancia').val(), 1, $('#txt_codigo').val(), $('#txt_nombre').val(), $('#txt_clase').next().children('input').val(), $('#txt_negocio').next().children('input').val(), $('#txt_unidad').next().children('input').val());
    }

    function MngMercancia(page, action) {

        var oCM = getCliente_mercancia();

        $.ajax({
            type: "POST",
            url: '/handlers/Catalog.ashx?catalogo=cliente_mercancia&opt=' + action,
            data: JSON.stringify(oCM),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            complete: function () {

            },
            success: function (data) {
                if (data.Id > 0)
                    alert('Se guardo correctamente el registro');
                $("#ctrlClienteMercancia").dialog('close');
                if (page != null) {
                    page.Recall(data, page.CtrlCM);
                }
                // window.location.href = 'frmMaquila.aspx?_fk=' + id_entrada + "&_pk=" + id_entrada_inventario;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var oErrorMessage = new ErrorMessage();
                oErrorMessage.SetError(jqXHR.responseText);
                oErrorMessage.Init();
            }
        });

    }
}