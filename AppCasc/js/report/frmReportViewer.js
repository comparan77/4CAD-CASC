﻿var frmReportViewer = function () {

    this.Init = function () {
        initControls();
    }

    function initControls() {
        $('#ctl00_body_btnGetRpt').button();
        $('#ctl00_body_txt_fecha_ini, #ctl00_body_txt_fecha_fin').datepicker({
            'dateFormat': 'dd/mm/yy'
        })
    }
}

var master = new webApp.Master;
var pag = new frmReportViewer();
master.Init(pag);