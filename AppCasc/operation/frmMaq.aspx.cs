﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModelCasc.operation;

namespace AppCasc.operation
{
    public partial class frmMaq : System.Web.UI.Page
    {
        protected Orden_trabajo VSOrdTbj
        {
            get
            {
                object o = ViewState["VSOrdTbj"];
                return o == null ? null : (Orden_trabajo)o;
            }
            set
            {
                ViewState["VSOrdTbj"] = value;
            }
        }

        private void loadFirstTime()
        {
            VSOrdTbj = new Orden_trabajo();
            string folio = string.Empty;
            if (Request.QueryString["folio"] != null)
            {
                folio = Request.QueryString["folio"].ToString();
                txt_folio.Text = folio;
                txt_folio_changed(txt_folio, null);
            }
        }

        private void fillInfo()
        {
            txt_fecha.Text = VSOrdTbj.Fecha.ToShortDateString();
            hf_id_orden_trabajo.Value = VSOrdTbj.Id.ToString();
            grd_servicios.DataSource = VSOrdTbj.PLstOTSer;
            grd_servicios.DataBind();
        }

        private void clearInfo()
        {
            hf_id_orden_trabajo.Value = string.Empty;
            txt_fecha.Text = string.Empty;
            grd_servicios.DataSource = null;
            grd_servicios.DataBind();
            grd_pasos.DataSource = null;
            grd_pasos.DataBind();
        }

        protected void txt_folio_changed(object sender, EventArgs args)
        {
            try
            {
                VSOrdTbj = MaquilaCtrl.OrdenTrabajoGet(txt_folio.Text.Trim().ToUpper());
                fillInfo();
            }
            catch (Exception e)
            {
                clearInfo();
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void grd_servicios_row_command(object sender, GridViewCommandEventArgs args)
        {
            try
            {
                int index = Convert.ToInt32(args.CommandArgument);
                int Id_ord_tbj_ser;
                int.TryParse(grd_servicios.DataKeys[index][0].ToString(), out Id_ord_tbj_ser);
                grd_pasos.DataSource = null;
                grd_pasos.DataBind();

                switch (args.CommandName)
                {
                    case "lnkPasos":
                        grd_pasos.DataSource = VSOrdTbj.PLstOTSer.Find(p => p.Id == Id_ord_tbj_ser).PLstPasos;
                        grd_pasos.DataBind();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void Page_Load(object sender, EventArgs args)
        {
            try
            {
                if (!IsPostBack)
                    loadFirstTime();
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }
    }
}