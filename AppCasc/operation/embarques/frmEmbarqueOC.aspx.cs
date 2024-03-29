﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModelCasc.webApp;
using ModelCasc.operation;
using Model;
using ModelCasc.catalog;
using Newtonsoft.Json;

namespace AppCasc.operation.embarques
{
    public partial class frmEmbarqueOC : System.Web.UI.Page
    {
        private Salida_orden_carga SSalidaOC
        {
            set
            {
                if (Session["SSalida_ord_carga"] != null)
                    Session.Remove("SSalida_ord_carga");
                Session.Add("SSalida_ord_carga", value);
            }
        }

        private void loadFirstTime()
        {
            try
            {
                int Id_salida_orden_carga = 0;
                if (Request.QueryString["_kp"] != null)
                {
                    int.TryParse(Request.QueryString["_kp"].ToString(), out Id_salida_orden_carga);
                    printSalida(Id_salida_orden_carga);
                }

                if (Request.QueryString["_octc"] != null)
                {
                    int.TryParse(Request.QueryString["_octc"].ToString(), out Id_salida_orden_carga);
                    printTransCond(Id_salida_orden_carga);
                }

                ControlsMng.fillBodega(ddlBodega);
                fillControls();
                txt_fecha.Text = DateTime.Today.ToString("dd MMM yy");
            }
            catch
            {
                throw;
            }
        }

        private void fillControls()
        {
            try
            {
                ddlBodega.SelectedValue = ((MstCasc)this.Master).getUsrLoged().Id_bodega.ToString();
                ControlsMng.fillCortinaByBodega(ddlCortina, Convert.ToInt32(ddlBodega.SelectedValue));
                ControlsMng.fillCliente(ddlCliente);
                ControlsMng.fillTransporte(ddl_linea);
                ControlsMng.fillTipoTransporte(ddl_tipo);
                ControlsMng.fillCustodia(ddlCustodia);
                ControlsMng.fillVigilanciaByBodega(ddlVigilante, Convert.ToInt32(ddlBodega.SelectedValue));

                int IdCliente = 0;
                int.TryParse(ddlCliente.SelectedValue, out IdCliente);
            }
            catch (Exception)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void validarTipoTransporte(int IdTransporteTipo)
        {
            try
            {
                if (IdTransporteTipo > 0)
                {
                    Transporte_tipoMng oMng = new Transporte_tipoMng();
                    Transporte_tipo o = new Transporte_tipo();
                    o.Id = IdTransporteTipo;
                    oMng.O_Transporte_tipo = o;
                    oMng.selById();

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

        private void saveSalida()
        {
            try
            {
                int numero;
                Salida_orden_carga oSOC = new Salida_orden_carga();
                oSOC.Id = Convert.ToInt32(hf_id_salida_orden_carga.Value);
                List<Salida> lstSalidas = new List<Salida>();

                int.TryParse(ddlCliente.SelectedValue, out numero);

                List<Salida_orden_carga_tc> lstSalTranCond = JsonConvert.DeserializeObject<List<Salida_orden_carga_tc>>(hf_condiciones_transporte.Value);
                numero = TransporteCtrl.TransCondCliGetNumCond(numero, false, true);
                if (lstSalTranCond.Count != numero)
                    throw new Exception("Es necesario proporcionar TODAS LAS CONDICIONES del transporte.");

                oSOC.PLstSalOCTransCond = lstSalTranCond;
                oSOC.Observaciones_tranpsorte = txt_comentarios.Text.Trim();

                Salida oS = getFormValues();
                List<Salida_compartida> lstSalComp = new List<Salida_compartida>();
                bool cumpleCondTran = lstSalTranCond.Count(p => p.Si_no == false) == 0;

                numero = 0;
                for (int iRow = 0; iRow < grd_rem.Rows.Count; iRow++)
                {
                    GridViewRow row = grd_rem.Rows[iRow];
                    Salida o = new Salida();
                    o = (Salida) oS.Clone();
                    o.Referencia = row.Cells[0].Text;
                    o.PLstSalTransCond = new List<Salida_transporte_condicion>();
                    HiddenField hfJsonDoc = row.FindControl("hf_JsonDocumentos") as HiddenField;
                    o.PLstSalDoc = JsonConvert.DeserializeObject<List<Salida_documento>>(hfJsonDoc.Value);
                    if (o.PLstSalDoc == null)
                        o.PLstSalDoc = new List<Salida_documento>();

                    //Numero de pallet
                    //TextBox txt_no_pallet = row.FindControl("txt_no_pallet") as TextBox;
                    int.TryParse(CommonFunctions.NumbersOnly(row.Cells[2].Text), out numero);
                    o.No_pallet = numero;
                    numero = 0;

                    //Numero de bulto
                    int.TryParse(CommonFunctions.NumbersOnly(row.Cells[3].Text), out numero);
                    o.No_bulto = numero;
                    numero = 0;

                    //Numero de pieza
                    int.TryParse(CommonFunctions.NumbersOnly(row.Cells[4].Text), out numero);
                    o.No_pieza = numero;
                    numero = 0;

                    //Mercancia
                    TextBox txt_mercancia = row.FindControl("txt_mercancia") as TextBox;
                    o.Mercancia = txt_mercancia.Text;

                    //observaciones
                    //Mercancia
                    TextBox txt_observaciones = row.FindControl("txt_observaciones") as TextBox;
                    o.Observaciones = txt_observaciones.Text.Trim();

                    //Forma (única o parcial)
                    HiddenField hf_forma = row.FindControl("hf_forma") as HiddenField;
                    Salida_parcial oSP = new Salida_parcial();
                    switch (Convert.ToInt32(hf_forma.Value))
                    {
                        case 0:
                            o.Es_unica = true;
                            break;
                        case 1:
                            oSP.Referencia = o.Referencia;
                            oSP.Es_ultima = false;
                            oSP.Id_usuario = o.PUsuario.Id;
                            o.PSalPar = oSP;
                            o.Es_unica = false;
                            break;
                        case -1:
                            oSP.Referencia = o.Referencia;
                            oSP.Es_ultima = true;
                            oSP.Id_usuario = o.PUsuario.Id;
                            o.PSalPar = oSP;
                            o.Es_unica = false;
                            break;
                    }

                    //Compartida
                    Salida_compartida oSC = new Salida_compartida();
                    oSC.Id_usuario = o.PUsuario.Id;
                    oSC.Capturada = false;
                    oSC.Referencia = o.Referencia;
                    lstSalComp.Add(oSC);
                    lstSalidas.Add(o);
                }

                if (lstSalComp.Count > 1)
                    foreach (Salida itemS in lstSalidas)
                    {
                        itemS.PLstSalComp = lstSalComp.FindAll(p => string.Compare(p.Referencia, itemS.Referencia) != 0);
                    }

                oSOC.LstSalida = lstSalidas;
                if (cumpleCondTran)
                {
                    SalidaCtrl.salidaAddFromLst(oSOC);
                    Response.Redirect("frmEmbarqueOC.aspx?_kp=" + hf_id_salida_orden_carga.Value);
                }
                else
                {
                    Salida oSTmp = oSOC.LstSalida.First();
                    oSTmp.PCliente = CatalogCtrl.Cliente_GetById(oSTmp.Id_cliente);
                    if (oSTmp.PLstSalComp == null)
                        oSTmp.PLstSalComp = new List<Salida_compartida>();

                    oSTmp.PTransporteTipo = CatalogCtrl.Transporte_tipo_getyById(oSTmp.Id_transporte_tipo);

                    SSalidaOC = oSOC;
                    Response.Redirect("frmEmbarqueOC.aspx?_octc=" + hf_id_salida_orden_carga.Value);
                }
            }
            catch
            {
                throw;
            }
        }

        private void printSalida(int Id_orden_carga)
        {
            string path = string.Empty;
            string pathImg = string.Empty;
            string virtualPath = string.Empty;
            ((MstCasc)this.Master).getUsrLoged().Id_print = Id_orden_carga.ToString();
            try
            {
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "openRpt", "<script type='text/javascript'>window.open('../frmReporter.aspx?rpt=ordCargaSal','_blank', 'toolbar=no');</script>");
            }
            catch
            {
                throw;
            }
        }

        private void printTransCond(int Id_orden_carga)
        {
            try
            {
                ((MstCasc)this.Master).getUsrLoged().Id_print = Id_orden_carga.ToString();
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "openRpt", "<script type='text/javascript'>window.open('../frmReporter.aspx?rpt=ordCargaSalTra','_blank', 'toolbar=no');</script>");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// En caso de que la búsqueda no devuelva ningún valor, el pie del repetidor mostrará
        /// la leyenda en la cual se indica al usuario que no ha resultados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void rep_resultados_ItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            try
            {
                if (((Repeater)sender).Items.Count < 1)
                {
                    if (args.Item.ItemType == ListItemType.Footer)
                    {
                        Label lblFooter = (Label)args.Item.FindControl("lbl_resultados");
                        lblFooter.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void grd_rem_databound(object sender, GridViewRowEventArgs args)
        {
            if (args.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_documento = args.Row.FindControl("ddl_documento") as DropDownList;
                ControlsMng.fillDocumento(ddl_documento);
                ddl_documento.Items.Remove(ddl_documento.Items.FindByValue(hf_id_doc_req_by_cliente.Value));
            }
        }

        private void fillControlsToOC(Salida_orden_carga oSOC)
        {
            #region Bodega Ubicacion
            ddlBodega.SelectedValue = oSOC.Id_bodega_ubicacion.ToString();
            ControlsMng.fillCortinaByBodega(ddlCortina, Convert.ToInt32(ddlBodega.SelectedValue));
            #endregion

            #region Cita
            txt_folio_cita.Text = oSOC.PSalidaTrafico.Folio_cita;
            txt_cita_fecha_hora.Text = CommonFunctions.FormatDate(Convert.ToDateTime(oSOC.PSalidaTrafico.Fecha_cita), "dd \\de MMMM \\de yyyy") + " " + oSOC.PSalidaTrafico.Hora_cita.ToString();
            txt_destino.Text = oSOC.PSalidaTrafico.PSalidaDestino.Direccion;
            #endregion

            #region Documentos
            List<Cliente_documento> lstCteDoc = CatalogCtrl.Cliente_DocumentoFillLstByCliente(Convert.ToInt32(ddlCliente.SelectedValue));
            hf_id_doc_req_by_cliente.Value = lstCteDoc.First().Id_documento.ToString();
            grd_rem.DataSource = oSOC.LstRem;
            grd_rem.DataBind();
            #endregion

            #region Transporte
            ddl_linea.SelectedValue = Convert.ToInt32(oSOC.PSalidaTrafico.Id_transporte).ToString();
            ddl_tipo.SelectedValue = Convert.ToInt32(oSOC.PSalidaTrafico.Id_transporte_tipo_cita).ToString();
            txt_placa.Text = oSOC.PSalidaTrafico.Placa;
            txt_caja.Text = oSOC.PSalidaTrafico.Caja;
            txt_caja_1.Text = oSOC.PSalidaTrafico.Caja1;
            txt_caja_2.Text = oSOC.PSalidaTrafico.Caja2;
            txt_operador.Text = oSOC.PSalidaTrafico.Operador;
            validarTipoTransporte(Convert.ToInt32(oSOC.PSalidaTrafico.Id_transporte_tipo_cita));
            #endregion

            #region Vigilancia
            ControlsMng.fillVigilanciaByBodega(ddlVigilante, Convert.ToInt32(ddlBodega.SelectedValue));
            #endregion
        }

        protected void click_result(object sender, RepeaterCommandEventArgs args)
        {
            try
            {
                int IdOc = 0;
                int.TryParse(args.CommandArgument.ToString(), out IdOc);
                hf_id_salida_orden_carga.Value = IdOc.ToString();
                Salida_orden_carga oSOC = SalidaCtrl.OrdenCargaGetById(IdOc);
                fillControlsToOC(oSOC);
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void btn_buscar_click(object sender, EventArgs args)
        {
            try
            {
                List<Salida_orden_carga> lst = SalidaCtrl.OrdenCargaGetLstByFolio(txt_dato.Text.Replace(" ", "").Trim());
                rep_resultados.DataSource = lst;
                rep_resultados.DataBind();
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        protected void ddlCliente_changed(object sender, EventArgs args)
        {
            int IdCliente = 0;
            int.TryParse(ddlCliente.SelectedValue, out IdCliente);
            if (IdCliente != 1)
                Response.Redirect("~/operation/frmSalidas.aspx?_idCte=" + IdCliente.ToString());
                
        }

        protected void btnGuardar_click(object sender, EventArgs args)
        {
            try
            {
                saveSalida();
            }
            catch (Exception e)
            {
                ((MstCasc)this.Master).setError = e.Message;
            }
        }

        private Salida getFormValues()
        {
            Salida oS = new Salida();
            int numero;

            //Cliente
            int.TryParse(ddlCliente.SelectedValue, out numero);
            oS.Id_cliente = numero;
            numero = 0;

            //Usuario
            oS.PUsuario = ((MstCasc)this.Master).getUsrLoged();

            //Bodega
            int.TryParse(ddlBodega.SelectedValue, out numero);
            oS.Id_bodega = numero;
            numero = 0;

            //Fecha
            oS.Fecha = DateTime.Today;

            //Hora
            oS.Hora_salida = txt_hora_salida.Text;

            //Cortina
            int.TryParse(ddlCortina.SelectedValue, out numero);
            oS.Id_cortina = numero;
            numero = 0;

            //Destino
            oS.Destino = txt_destino.Text;

            //Linea de Transporte
            int.TryParse(ddl_linea.SelectedValue, out numero);
            oS.Id_transporte = numero;
            numero = 0;

            //Tipo de transporte
            int.TryParse(ddl_tipo.SelectedValue, out numero);
            oS.Id_transporte_tipo = numero;
            numero = 0;

            //Placa
            oS.Placa = txt_placa.Text;

            //Caja
            oS.Caja = txt_caja.Text;

            //Caja1
            oS.Caja1 = txt_caja_1.Text;

            //Caja2
            oS.Caja2 = txt_caja_2.Text;

            //Sello
            oS.Sello = txt_sello.Text;

            //Talon
            oS.Talon = txt_talon.Text;

            //Custodia
            int.TryParse(ddlCustodia.SelectedValue, out numero);
            oS.Id_custodia = numero;
            numero = 0;

            //Operador de la custodia
            oS.Operador = txt_operador.Text;

            //Hora carga
            oS.Hora_carga = txt_hora_carga.Text;

            //Vigilante
            oS.Vigilante = ddlVigilante.SelectedItem.Text;

            //Orden de carga
            oS.Id_salida_orden_carga = Convert.ToInt32(hf_id_salida_orden_carga.Value);

            return oS;
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