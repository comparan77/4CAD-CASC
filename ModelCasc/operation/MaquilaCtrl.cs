﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;
using System.IO;
using ModelCasc.catalog;
using ModelCasc.operation.liverpool;

namespace ModelCasc.operation
{
    public class MaquilaCtrl
    {
        #region Orden Trabajo

        public static Orden_trabajo OrdenTrabajoAdd(Orden_trabajo o)
        {
            IDbTransaction trans = null;
            try
            {
                trans = GenericDataAccess.BeginTransaction();
                o.Folio = FolioCtrl.getFolio(enumTipo.OT, trans);
                Orden_trabajoMng oMng = new Orden_trabajoMng() { O_Orden_trabajo = o };
                oMng.add(trans);
                Orden_trabajo_servicioMng oOTSMng = new Orden_trabajo_servicioMng();
                foreach (Orden_trabajo_servicio itemOTS in o.PLstOTSer)
                {
                    itemOTS.Id_orden_trabajo = o.Id;
                    oOTSMng.O_Orden_trabajo_servicio = itemOTS;
                    oOTSMng.add(trans);
                }
                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
            return o;
        }

        public static List<Orden_trabajo> OrdenTrabajoGetLst()
        {
            List<Orden_trabajo> lst = new List<Orden_trabajo>();
            string folioError = string.Empty;
            try
            {
                Orden_trabajoMng oMng = new Orden_trabajoMng();
                oMng.fillOpen();
                lst = oMng.Lst;

                Orden_trabajo_servicioMng oOTSMng = new Orden_trabajo_servicioMng();

                Entrada_liverpoolMng oELMng = new Entrada_liverpoolMng();
                Entrada_liverpool oEL;

                Etiqueta_tipoMng oETMng = new Etiqueta_tipoMng();

                MaquilaMng oMaqMng = new MaquilaMng();
                Maquila_pasoMng oMaqPasoMng = new Maquila_pasoMng();

                Entrada oE;
                EntradaMng oEMng = new EntradaMng();


                foreach (Orden_trabajo itemOT in lst)
                {
                    itemOT.PLstOTSer = new List<Orden_trabajo_servicio>();
                    Orden_trabajo_servicio oOTS = new Orden_trabajo_servicio() { Id_orden_trabajo = itemOT.Id };
                    oOTSMng.O_Orden_trabajo_servicio = oOTS;
                    oOTSMng.fillLstByIdOT();
                    itemOT.Servicios = oOTSMng.Lst.Count();

                    oEL = new Entrada_liverpool();
                    oEL.Trafico = itemOT.Referencia;
                    oELMng.O_Entrada_liverpool = oEL;
                    oELMng.selByTrafico();

                    if (oEL.Id_entrada > 0)
                    {
                        oE = new Entrada();
                        oE.Id = oEL.Id_entrada;
                        oEMng.O_Entrada = oE;
                        oEMng.selById();

                        itemOT.PEnt = oE;
                    }
                    foreach (Orden_trabajo_servicio itemOTS in oOTSMng.Lst)
                    {
                        Etiqueta_tipo oET = new Etiqueta_tipo() { Id = itemOTS.Id_etiqueta_tipo };
                        oETMng.O_Etiqueta_tipo = oET;
                        oETMng.selById();
                        itemOTS.PEtiquetaTipo = oET;

                        Maquila oMaq = new Maquila() { Id_ord_tbj_srv = itemOTS.Id };
                        oMaqMng.O_Maquila = oMaq;
                        oMaqMng.fillLstByOTS();
                        itemOTS.PLstMaq = oMaqMng.Lst;

                        itemOTS.PiezasMaq = itemOTS.PLstMaq.Sum(p => p.Piezas);

                        Maquila_paso oMaqPaso = new Maquila_paso() { Id_ord_tbj_srv = itemOTS.Id };
                        oMaqPasoMng.O_Maquila_paso = oMaqPaso;
                        oMaqPasoMng.fillByIdOTS();
                        itemOTS.PLstPasos = oMaqPasoMng.Lst;

                        switch (itemOTS.Id_servicio)
                        {
                            case 1: //etiqueta de precio
                                oEL = new Entrada_liverpool() { Trafico = itemOTS.Ref1, Pedido = Convert.ToInt32(itemOTS.Ref2), Parcial = itemOTS.Parcial };
                                oELMng.O_Entrada_liverpool = oEL;
                                oELMng.selByUniqueKey();
                                itemOTS.PEntLiv = oEL;
                                break;
                            default:
                                break;
                        }
                        itemOT.PLstOTSer.Add(itemOTS);
                    }
                }
            }
            catch
            {

                throw new Exception(folioError);
            }
            return lst;
        }

        public static Orden_trabajo OrdenTrabajoGet(string folio)
        {
            Orden_trabajo o = new Orden_trabajo() { Folio = folio };
            try
            {
                Orden_trabajoMng oMng = new Orden_trabajoMng() { O_Orden_trabajo = o };
                oMng.selByFolio();

                Orden_trabajo_servicioMng oOTSMng = new Orden_trabajo_servicioMng() { O_Orden_trabajo_servicio = new Orden_trabajo_servicio() { Id_orden_trabajo = o.Id } };
                oOTSMng.fillLstByIdOT();

                o.PLstOTSer = oOTSMng.Lst;

                Entrada_liverpoolMng oELMng = new Entrada_liverpoolMng();
                MaquilaMng oMMng = new MaquilaMng();
                Maquila_pasoMng oMPMng = new Maquila_pasoMng();
                ServicioMng oSMng = new ServicioMng();
                foreach (Orden_trabajo_servicio itemOTS in o.PLstOTSer)
                {
                    int ref2 = 0;
                    int.TryParse(itemOTS.Ref2, out ref2);

                    Entrada_liverpool oEL = new Entrada_liverpool() { Trafico = itemOTS.Ref1, Pedido = ref2 };
                    oELMng.O_Entrada_liverpool = oEL;
                    oELMng.selByUniqueKey();
                    itemOTS.PEntLiv = oEL;

                    Servicio oS = new Servicio() { Id = itemOTS.Id_servicio };
                    oSMng.O_Servicio = oS;
                    oSMng.selById();
                    itemOTS.PServ = oS;

                    Maquila oM = new Maquila() { Id_ord_tbj_srv = itemOTS.Id };
                    oMMng.O_Maquila = oM;
                    oMMng.fillLstByOTS();
                    itemOTS.PLstMaq = oMMng.Lst;

                    itemOTS.PalletMaq = itemOTS.PLstMaq.Sum(p => p.Pallets);
                    itemOTS.BultosMaq = itemOTS.PLstMaq.Sum(p => p.Bultos);
                    itemOTS.PiezasMaq = itemOTS.PLstMaq.Sum(p => p.Piezas);
                    int dif = itemOTS.Piezas - itemOTS.PiezasMaq;
                    itemOTS.Faltantes = 0;
                    itemOTS.Sobrantes = 0;
                    if (itemOTS.PiezasMaq > 0)
                    {
                        if (dif > 0)
                        {
                            itemOTS.Faltantes = dif;
                        }
                        else
                        {
                            itemOTS.Sobrantes = Math.Abs(dif);
                        }
                    }

                    Maquila_paso oMP = new Maquila_paso() { Id_ord_tbj_srv = itemOTS.Id };
                    oMPMng.O_Maquila_paso = oMP;
                    oMPMng.fillByIdOTS();
                    itemOTS.PLstPasos = oMPMng.Lst;

                    int numPaso = 1;
                    foreach (Maquila_paso itemMP in oMPMng.Lst)
                    {
                        itemMP.NumPaso = numPaso;
                        numPaso++;
                    }

                    itemOTS.PasosMaq = itemOTS.PLstPasos.Count();
                }
            }
            catch
            {
                throw;
            }
            return o;
        }

        private static void OrdenTrabajoChangeStatus(int id_orden_trabajo, bool cerrada)
        {
            try
            {
                Orden_trabajoMng oMng = new Orden_trabajoMng() { O_Orden_trabajo = new Orden_trabajo() { Id = id_orden_trabajo, Cerrada = cerrada } };
                oMng.udtStatus();
            }
            catch
            {
                
                throw;
            }
        }

        public static void OrdenTrabajoOpen(int id)
        {
            try
            {
                OrdenTrabajoChangeStatus(id, false);
            }
            catch
            {
                
                throw;
            }
        }

        public static void OrdenTrabajoClose(int id)
        {
            try
            {
                OrdenTrabajoChangeStatus(id, true);
            }
            catch
            {
                
                throw;
            }
        }

        public static List<Orden_trabajo> OrdenTrabajoGetLstCloseOrOpen(bool cerrada)
        {
            List<Orden_trabajo> lst = new List<Orden_trabajo>();
            try
            {
                Orden_trabajoMng oMng = new Orden_trabajoMng() { O_Orden_trabajo = new Orden_trabajo() { Cerrada = cerrada } };
                oMng.fillLstCloseOrOpen();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Maquila

        public static object MaquilaAddLst(List<Maquila> lst, string pathImg)
        {
            int rowsAfected = 0;
            IDbTransaction trans = null;
            try
            {
                MaquilaMng oMng = new MaquilaMng();
                Maquila_pasoMng oMPMng = new Maquila_pasoMng();
                trans = GenericDataAccess.BeginTransaction();
                foreach (Maquila itemMaq in lst)
                {
                    oMng.O_Maquila = itemMaq;
                    oMng.add(trans);

                    //si no cuenta con captura de pasos
                    string path = Path.Combine(pathImg, itemMaq.Id_ord_tbj_srv + @"\");
                    int numFoto = 1;
                    oMPMng.O_Maquila_paso = new Maquila_paso() { Id_ord_tbj_srv = itemMaq.Id_ord_tbj_srv };
                    oMPMng.fillByIdOTS(trans);
                    if (oMPMng.Lst.Count == 0)
                        foreach (Maquila_paso itemMP in itemMaq.PLstPasos)
                        {
                            string foto64 = itemMP.Foto64;
                            itemMP.Foto64 = Path.Combine(itemMaq.Id_ord_tbj_srv + @"\" + numFoto + ".jpg");
                            oMPMng.O_Maquila_paso = itemMP;
                            oMPMng.add(trans);
                            CommonCtrl.AddImgToDirectory(path, numFoto.ToString(), foto64);
                            numFoto++;
                        }
                    rowsAfected++;
                }
                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
            return rowsAfected;
        }

        #endregion

    }
}
