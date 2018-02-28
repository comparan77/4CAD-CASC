﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModelCasc.webApp;
using ModelCasc.catalog;

namespace AppCasc.operation
{
    public partial class frmAudUni : System.Web.UI.Page
    {
        private void loadFirstTime()
        {
            try
            {
                ControlsMng.fillTransporte(ddlTransporte);
                ControlsMng.fillTipoTransporte(ddlTipo_Transporte, ddlTransporte);
                int IdTransporteTipo = 0;
                int.TryParse(ddlTipo_Transporte.SelectedValue, out IdTransporteTipo);
                validarTipo(IdTransporteTipo);
            }
            catch
            {
                throw;
            }
            
        }

        private void validarTipo(int IdTransporteTipo)
        {
            try
            {
                //if (IdTransporteTipo > 0 && string.Compare(hf_click_save.Value, "1") != 0)
                if (IdTransporteTipo > 0)
                {

                    Transporte_tipo o = CatalogCtrl.Transporte_tipo_getyById(IdTransporteTipo);
                    
                    //rv_total_carga_max.MinimumValue = "0";
                    //rv_total_carga_max.MaximumValue = o.Peso_maximo.ToString();
                    //rv_total_carga_max.ErrorMessage = "El peso excede los " + o.Peso_maximo.ToString() + " Kg, para el tipo de transrpote selecccionado";

                    txt_placa.Text = string.Empty;
                    txt_placa.ReadOnly = (!o.Requiere_placa);
                    txt_caja.Text = string.Empty;
                    txt_caja.ReadOnly = (!o.Requiere_caja);
                    txt_caja_1.Text = string.Empty;
                    txt_caja_1.ReadOnly = (!o.Requiere_caja1);
                    txt_caja_2.Text = string.Empty;
                    txt_caja_2.ReadOnly = (!o.Requiere_caja2);

                    if (txt_placa.ReadOnly)
                        txt_placa.Text = "N.A.";
                    if (txt_caja.ReadOnly)
                        txt_caja.Text = "N.A.";
                    if (txt_caja_1.ReadOnly)
                        txt_caja_1.Text = "N.A.";
                    if (txt_caja_2.ReadOnly)
                        txt_caja_2.Text = "N.A.";
                }
            }
            catch
            {
                throw;
            }
        }

        protected void ddlTransporte_changed(object sender, EventArgs args)
        {
            try
            {
                ControlsMng.fillTipoTransporte(ddlTipo_Transporte, ddlTransporte);
                ddlTipo_Transporte_changed(null, null);
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void ddlTipo_Transporte_changed(object sender, EventArgs args)
        {
            try
            {
                int IdTransporteTipo = 0;
                int.TryParse(ddlTipo_Transporte.SelectedValue, out IdTransporteTipo);
                validarTipo(IdTransporteTipo);
                upDatosVehiculo.Update();
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void btnGuardar_click(object sender, EventArgs args)
        {
            try
            {
                //Response.Redirect("frmEmbarqueWH.aspx?_kp=" + oS.Id);
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
                try
                {
                    loadFirstTime();
                }
                catch (Exception e)
                {
                    ((MstCasc)this.Master).setError = e.Message;
                }
        }
    }
}