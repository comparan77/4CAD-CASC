﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelCasc.operation;
using Newtonsoft.Json;
using System.Web.Security;
using System.Data;
using Model;
using ModelCasc.catalog.almacen;

namespace ModelCasc.catalog
{
    public class CatalogCtrl
    {
        private static string _strJson = string.Empty;

        #region Bodega
        public static Bodega BodegaGet(int Id)
        {
            Bodega o = new Bodega() { Id = Id };
            try
            {
                BodegaMng oMng = new BodegaMng() { O_Bodega = o };
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }
        #endregion

        #region Pivote

        public static List<Pivote> PivoteGetColumnNames()
        {
            List<Pivote> lst;
            try
            {
                PivoteMng oPMng = new PivoteMng();
                oPMng.fillLst();
                lst = oPMng.Lst;
            }
            catch (Exception)
            {

                throw;
            }
            return lst;
        }

        #endregion

        #region Aduana

        public static List<Aduana> Aduanafill()
        {
            List<Aduana> lst = new List<Aduana>();
            try
            {
                AduanaMng oMng = new AduanaMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Aduana> AduanafillEvenInactive()
        {
            List<Aduana> lst = new List<Aduana>();
            try
            {
                AduanaMng oMng = new AduanaMng();
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void AduanaChangeStatus(Aduana o, bool status)
        {
            try
            {
                AduanaMng oMng = new AduanaMng();
                oMng.O_Aduana = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Aduana AduanaGet(int Id)
        {
            Aduana o = new Aduana();
            try
            {
                o.Id = Id;
                AduanaMng oMng = new AduanaMng();
                oMng.O_Aduana = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static Aduana AduanaGetByCodigo(string codigo)
        {
            Aduana o = new Aduana();
            try
            {
                o.Codigo = codigo;
                AduanaMng oMng = new AduanaMng();
                oMng.O_Aduana = o;
                oMng.selByCodigo();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void AduanaAdd(Aduana o)
        {
            try
            {
                AduanaMng oMng = new AduanaMng();
                oMng.O_Aduana = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void AduanaUdt(Aduana o)
        {
            try
            {
                AduanaMng oMng = new AduanaMng();
                oMng.O_Aduana = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Ubicacion

        public static List<Ubicacion> Ubicacionfill()
        {
            List<Ubicacion> lst = new List<Ubicacion>();
            try
            {
                UbicacionMng oMng = new UbicacionMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Ubicacion> UbicacionfillEvenInactive()
        {
            List<Ubicacion> lst = new List<Ubicacion>();
            try
            {
                UbicacionMng oMng = new UbicacionMng();
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch 
            {
                throw;
            }
            return lst;
        }

        public static void UbicacionChangeStatus(Ubicacion o, bool status)
        {
            try
            {
                UbicacionMng oMng = new UbicacionMng();
                oMng.O_Ubicacion = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Ubicacion UbicacionGet(int Id)
        {
            Ubicacion o = new Ubicacion();
            try
            {
                o.Id = Id;
                UbicacionMng oMng = new UbicacionMng();
                oMng.O_Ubicacion = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void UbicacionAdd(Ubicacion o)
        {
            try
            {
                UbicacionMng oMng = new UbicacionMng();
                oMng.O_Ubicacion = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void UbicacionUdt(Ubicacion o)
        {
            try
            {
                UbicacionMng oMng = new UbicacionMng();
                oMng.O_Ubicacion = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region NOM

        public static List<Nom> Nomfill()
        {
            List<Nom> lst = new List<Nom>();
            try
            {
                NomMng oMng = new NomMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Nom> NomfillEvenInactive()
        {
            List<Nom> lst = new List<Nom>();
            try
            {
                NomMng oMng = new NomMng();
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void NomChangeStatus(Nom o, bool status)
        {
            try
            {
                NomMng oMng = new NomMng();
                oMng.O_Nom = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Nom NomGet(int Id)
        {
            Nom o = new Nom();
            try
            {
                o.Id = Id;
                NomMng oMng = new NomMng();
                oMng.O_Nom = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void NomAdd(Nom o)
        {
            try
            {
                NomMng oMng = new NomMng();
                oMng.O_Nom = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void NomUdt(Nom o)
        {
            try
            {
                NomMng oMng = new NomMng();
                oMng.O_Nom = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Documentos

        public static string DocumentoLstToJson()
        {
            DocumentoMng oMng = new DocumentoMng();
            oMng.fillLst();
            return JsonConvert.SerializeObject(oMng.Lst, Formatting.Indented);
        }

        #endregion

        #region Cliente

        public static List<Cliente> Cliente_GetAll()
        {
            List<Cliente> lst = new List<Cliente>();
            try
            {
                ClienteMng oMng = new ClienteMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch (Exception)
            {
                
                throw;
            }
            return lst;
        }

        public static Cliente Cliente_GetByIdEntrada(int idEntrada)
        {
            Cliente oC = new Cliente();
            try
            {
                Entrada oE = new Entrada() { Id = idEntrada };
                EntradaMng oEMng = new EntradaMng() { O_Entrada = oE };
                oEMng.selById();

                oC = new Cliente() { Id = oE.Id_cliente };
                ClienteMng oCMng = new ClienteMng() { O_Cliente = oC };
                oCMng.selById();

            }
            catch
            {

                throw;
            }
            return oC;
        }

        public static Cliente Cliente_GetById(int id)
        {
            Cliente oC = new Cliente();
            ClienteMng oCMng = new ClienteMng();
            oC.Id = id;
            oCMng.O_Cliente = oC;
            try
            {
                oCMng.selById();
            }
            catch
            {
                throw;
            }

            return oC;
        }
        
        public static void Cliente_changeStatus(Cliente oC, bool status)
        {
            try
            {
                ClienteMng oCMng = new ClienteMng();

                oCMng.O_Cliente = oC;
                if (status)
                    oCMng.dlt();
                else
                    oCMng.reactive();
            }
            catch
            {
                
                throw;
            }
        }

        public static List<Cliente> Cliente_FillAllLst()
        {
            List<Cliente> lst = new List<Cliente>();
            try
            {
                ClienteMng oCMng = new ClienteMng();
                oCMng.fillAllLst();
                lst = oCMng.Lst;
            }
            catch 
            {
                
                throw;
            }
            return lst;
        }

        public static void Cliente_udt(Cliente oC)
        {
            IDbTransaction trans = null;
            try
            {
                trans = GenericDataAccess.BeginTransaction();
                ClienteMng oCMng = new ClienteMng();
                oCMng.O_Cliente = oC;
                oCMng.udt(trans);

                Cliente_documentoMng oCDMng = new Cliente_documentoMng();
                Cliente_documento oCDDlt = new Cliente_documento();
                oCDDlt.Id_cliente = oC.Id;
                oCDMng.O_Cliente_documento = oCDDlt;
                oCDMng.dltByCliente(trans);

                foreach (Cliente_documento oCD in oC.PLstDocReq)
                {
                    oCD.Id_cliente = oC.Id;
                    oCDMng.O_Cliente_documento = oCD;
                    oCDMng.add(trans);
                }

                Cliente_copia_operacionMng oCCOpMng = new Cliente_copia_operacionMng();
                oCCOpMng.dltByCliente(oC.Id, trans);
                foreach (Cliente_copia_operacion oCCOp in oC.PLstCopiaOp)
                {
                    oCCOp.Id_cliente = oC.Id;
                    oCCOpMng.O_Cliente_copia_operacion = oCCOp;
                    oCCOpMng.add(trans);
                }
                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
        }

        public static void Cliente_add(Cliente oC)
        {
            IDbTransaction trans = null;
            try
            {
                trans = GenericDataAccess.BeginTransaction();
                ClienteMng oCMng = new ClienteMng();
                oCMng.O_Cliente = oC;
                oCMng.add(trans);

                Cliente_documentoMng oCDMng = new Cliente_documentoMng();
                foreach (Cliente_documento oCD in oC.PLstDocReq)
                {
                    oCD.Id_cliente = oC.Id;
                    oCDMng.O_Cliente_documento = oCD;
                    oCDMng.add(trans);
                }

                Cliente_copia_operacionMng oCCOpMng = new Cliente_copia_operacionMng();
                foreach (Cliente_copia_operacion oCCOp in oC.PLstCopiaOp)
                {
                    oCCOp.Id_cliente = oC.Id;
                    oCCOpMng.O_Cliente_copia_operacion = oCCOp;
                    oCCOpMng.add(trans);
                }

                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
        }

        #endregion

        #region Cliente Documento

        public static string Cliente_DocumentoLstToJson()
        {
            Cliente_documentoMng oMng = new Cliente_documentoMng();
            oMng.fillLst();
            return JsonConvert.SerializeObject(oMng.Lst, Formatting.Indented);
        }

        public static List<Cliente_documento> Cliente_DocumentoFillLstByCliente(int id_cliente)
        {
            List<Cliente_documento> lst = new List<Cliente_documento>();
            try
            {
                Cliente_documentoMng oCDMng = new Cliente_documentoMng();
                Cliente_documento oCD = new Cliente_documento();
                oCD.Id_cliente = id_cliente;
                oCDMng.O_Cliente_documento = oCD;
                oCDMng.fillLstByCliente();
                lst = oCDMng.Lst;
            }
            catch 
            {
                
                throw;
            }
            return lst;
        }

        #endregion

        #region Cliente Codigo

        public static Cliente_codigo Cliente_codigoGet(int IdClienteGrupo)
        {
            Cliente_codigo o = new Cliente_codigo();
            try
            {
                o.Id_cliente_grupo = IdClienteGrupo;
                Cliente_codigoMng oMng = new Cliente_codigoMng();
                oMng.O_Cliente_codigo = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void Cliente_codigoUdt(Cliente_codigo o)
        {
            try
            {
                Cliente_codigoMng oMng = new Cliente_codigoMng();
                oMng.O_Cliente_codigo = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Cliente Grupo

        public static Cliente_grupo Cliente_grupoGet(int IdClienteGrupo)
        {
            Cliente_grupo o = new Cliente_grupo();
            try
            {
                Cliente_grupoMng oC_gMng = new Cliente_grupoMng();
                o.Id = IdClienteGrupo;
                oC_gMng.O_Cliente_grupo = o;
                oC_gMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        #endregion

        #region Cliente comprador

        public static List<Cliente_comprador> Cliente_compradorfill(int IdClienteGrupo)
        {
            List<Cliente_comprador> lst = new List<Cliente_comprador>();
            try
            {
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                Cliente_comprador o = new Cliente_comprador();
                o.Id_cliente_grupo = IdClienteGrupo;
                oMng.O_Cliente_comprador = o;
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Cliente_comprador> Cliente_compradorfillEvenInactive(int IdClienteGrupo)
        {
            List<Cliente_comprador> lst = new List<Cliente_comprador>();
            try
            {
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                Cliente_comprador o = new Cliente_comprador();
                o.Id_cliente_grupo = IdClienteGrupo;
                oMng.O_Cliente_comprador = o;
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void Cliente_compradorChangeStatus(Cliente_comprador o, bool status)
        {
            try
            {
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                oMng.O_Cliente_comprador = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Cliente_comprador Cliente_compradorGet(int Id)
        {
            Cliente_comprador o = new Cliente_comprador();
            try
            {
                o.Id = Id;
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                oMng.O_Cliente_comprador = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void Cliente_compradorAdd(Cliente_comprador o)
        {
            try
            {
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                oMng.O_Cliente_comprador = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void Cliente_compradorUdt(Cliente_comprador o)
        {
            try
            {
                Cliente_compradorMng oMng = new Cliente_compradorMng();
                oMng.O_Cliente_comprador = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Cliente Vendor

        public static List<Cliente_vendor> Cliente_vendorfillByName(string nombre)
        {
            List<Cliente_vendor> lst = new List<Cliente_vendor>();
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                Cliente_vendor o = new Cliente_vendor();
                o.Nombre = nombre;
                oMng.O_Cliente_vendor = o;
                oMng.fillLstByName();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }
        
        public static List<Cliente_vendor> Cliente_vendorfill(int IdClienteGrupo)
        {
            List<Cliente_vendor> lst = new List<Cliente_vendor>();
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                Cliente_vendor o = new Cliente_vendor();
                o.Id_cliente_grupo = IdClienteGrupo;
                oMng.O_Cliente_vendor = o;
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Cliente_vendor> Cliente_vendorfillEvenInactive(int IdClienteGrupo)
        {
            List<Cliente_vendor> lst = new List<Cliente_vendor>();
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                Cliente_vendor o = new Cliente_vendor();
                o.Id_cliente_grupo = IdClienteGrupo;
                oMng.O_Cliente_vendor = o;
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void Cliente_vendorChangeStatus(Cliente_vendor o, bool status)
        {
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                oMng.O_Cliente_vendor = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Cliente_vendor Cliente_vendorGet(int Id)
        {
            Cliente_vendor o = new Cliente_vendor();
            try
            {
                o.Id = Id;
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                oMng.O_Cliente_vendor = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void Cliente_vendorAdd(Cliente_vendor o)
        {
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                oMng.O_Cliente_vendor = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void Cliente_vendorUdt(Cliente_vendor o)
        {
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                oMng.O_Cliente_vendor = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        public static List<Cliente_vendor> Cliente_vendorfillByCliente(int IdClienteGrupo, string vendor = "")
        {
            List<Cliente_vendor> lst = new List<Cliente_vendor>();
            try
            {
                Cliente_vendorMng oMng = new Cliente_vendorMng();
                Cliente_vendor o = new Cliente_vendor();
                o.Id_cliente_grupo = IdClienteGrupo;
                if (vendor.Length > 0)
                    o.Codigo = vendor;
                oMng.O_Cliente_vendor = o;
                oMng.fillLstByVendor();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Cliente mercancia

        public static List<Cliente_mercancia> Cliente_mercanciafillByCliente(int IdClienteGrupo, string codigo = "")
        {
            List<Cliente_mercancia> lst = new List<Cliente_mercancia>();
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                Cliente_mercancia o = new Cliente_mercancia();
                o.Id_cliente_grupo = IdClienteGrupo;
                if (codigo.Length > 0)
                    o.Codigo = codigo;
                oMng.O_Cliente_mercancia = o;
                oMng.fillLstByCliente();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Cliente_mercancia> Cliente_mercanciafillEvenInactive(int IdClienteGrupo, string findBy = "")
        {
            List<Cliente_mercancia> lst = new List<Cliente_mercancia>();
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                Cliente_mercancia o = new Cliente_mercancia();
                o.Id_cliente_grupo = IdClienteGrupo;
                if (findBy.Length > 0)
                    o.Codigo = findBy;
                oMng.O_Cliente_mercancia = o;
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void Cliente_mercanciaChangeStatus(Cliente_mercancia o, bool status)
        {
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                oMng.O_Cliente_mercancia = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Cliente_mercancia Cliente_mercanciaGet(int Id)
        {
            Cliente_mercancia o = new Cliente_mercancia();
            try
            {
                o.Id = Id;
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                oMng.O_Cliente_mercancia = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void Cliente_mercanciaAdd(Cliente_mercancia o)
        {
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                oMng.O_Cliente_mercancia = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void Cliente_mercanciaUdt(Cliente_mercancia o)
        {
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                oMng.O_Cliente_mercancia = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        public static List<string> Cliente_mercanciaGetNegocios()
        {
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng();
                return oMng.getNegocioRecurrente();
            }
            catch
            {
                throw;
            }
        }

        public static Cliente_mercancia cliente_mercanciaFindByCode(Cliente_mercancia o)
        {
            try
            {
                Cliente_mercanciaMng oMng = new Cliente_mercanciaMng() { O_Cliente_mercancia = o };
                oMng.selByCode();
            }
            catch
            {
                throw;
            }
            return o;
        }

        #endregion

        #region Cliente mercancia cuenta

        public List<Cliente_mercancia_cuenta> Cliente_mercancia_cuentaFill()
        {
            List<Cliente_mercancia_cuenta> lst = new List<Cliente_mercancia_cuenta>();
            try
            {
                Cliente_mercancia_cuentaMng oMng = new Cliente_mercancia_cuentaMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Cliente Copias Operaciones

        public static List<Cliente_copia> ClienteCopiaLst()
        {
            List<Cliente_copia> lst = new List<Cliente_copia>();
            try
            {
                Cliente_copiaMng oMng = new Cliente_copiaMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Cliente_copia> ClienteCopiaOperacionLst(int id_operacion, int id_cliente)
        {
            List<Cliente_copia> lst = new List<Cliente_copia>();
            try
            {
                Cliente_copia_operacionMng oCCOMng = new Cliente_copia_operacionMng();
                Cliente_copia_operacion oCCO = new Cliente_copia_operacion() { Id_operacion = id_operacion, Id_cliente = id_cliente };
                oCCOMng.O_Cliente_copia_operacion = oCCO;
                oCCOMng.fillLstByCliente_operacion();
                List<Cliente_copia_operacion> lstCCO = oCCOMng.Lst;

                Cliente_copiaMng oCCMng = new Cliente_copiaMng();
                foreach (Cliente_copia_operacion itemCCO in lstCCO)
                {
                    Cliente_copia oCC = new Cliente_copia() { Id = itemCCO.Id_cliente_copia };
                    oCCMng.O_Cliente_copia = oCC;
                    oCCMng.selById();
                    lst.Add(oCC);
                }
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Tipos de Carga

        public static List<Tipo_carga> Tipo_cargafill()
        {
            List<Tipo_carga> lst = new List<Tipo_carga>();
            try
            {
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static List<Tipo_carga> Tipo_cargafillEvenInactive()
        {
            List<Tipo_carga> lst = new List<Tipo_carga>();
            try
            {
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.fillEvenInactive();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static void Tipo_cargaChangeStatus(Tipo_carga o, bool status)
        {
            try
            {
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.O_Tipo_carga = o;
                if (status)
                    oMng.dlt();
                else
                    oMng.reactive();
            }
            catch
            {
                throw;
            }
        }

        public static Tipo_carga Tipo_cargaGet(int Id)
        {
            Tipo_carga o = new Tipo_carga();
            try
            {
                o.Id = Id;
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.O_Tipo_carga = o;
                oMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static void Tipo_cargaAdd(Tipo_carga o)
        {
            try
            {
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.O_Tipo_carga = o;
                oMng.add();
            }
            catch
            {
                throw;
            }
        }

        public static void Tipo_cargaUdt(Tipo_carga o)
        {
            try
            {
                Tipo_cargaMng oMng = new Tipo_cargaMng();
                oMng.O_Tipo_carga = o;
                oMng.udt();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region usuario

        //public static bool UsuarioHasRol(Usuario o, enumRol rol)
        //{
        //    bool hasRol = false;
        //    List<string> roles = Roles.GetRolesForUser(o.Clave).ToList();
        //    hasRol = roles.Exists(p => string.Compare(p.ToString(), rol.ToString()) == 0);
        //    return hasRol;
        //}

        public static List<Usuario> usuarioFillAllList()
        {
            List<Usuario> lst = new List<Usuario>();
            try
            {
                UsuarioMng oUMng = new UsuarioMng();
                oUMng.fillAllLst();
                lst = oUMng.Lst;
            }
            catch 
            {
                
                throw;
            }
            return lst;
        }

        public static Usuario UsuarioSelByClaveContrasenia(string username, string password)
        {
            Usuario o = new Usuario();
            try
            {
                o.Clave = username;
                o.Contrasenia = password;
                UsuarioMng oUMng = new UsuarioMng() { O_Usuario = o };
                oUMng.selByClaveContrasenia();
                if (o.Id <= 0)
                    throw new Exception("El nombre de usuario y/o contraseña son incorrectos " + o.Id.ToString());
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static Usuario UsuarioSelById(int id_usuario)
        {
            Usuario o = new Usuario();
            try
            {
                o.Id = id_usuario;
                UsuarioMng oUMng = new UsuarioMng() { O_Usuario = o };
                oUMng.selById();
            }
            catch
            {
                throw;
            }
            return o;
        }

        public static Usuario UsuarioChangeStatus(int id_usuario, bool status)
        {
            Usuario oU = new Usuario();
            try
            {
                oU.Id = id_usuario;
                UsuarioMng oBMng = new UsuarioMng();
                oBMng.O_Usuario = oU;
                oBMng.selById();

                if (status)
                    oBMng.dlt();
                else
                    oBMng.reactive();
            }
            catch
            {
                throw;
            }
            return oU;
        }

        public static void usuarioAdd(Usuario oU)
        {
            IDbTransaction trans = null;
            try
            {
                trans = GenericDataAccess.BeginTransaction();
                UsuarioMng oUMng = new UsuarioMng();
                oU.Id_bodega = oU.PLstUsuarioBodega.First().Id_bodega;
                oUMng.O_Usuario = oU;
                oUMng.add(trans);

                Usuario_bodegaMng OUBMng = new Usuario_bodegaMng();
                foreach (Usuario_bodega itemUB in oU.PLstUsuarioBodega)
                {
                    itemUB.Id_usuario = oU.Id;
                    OUBMng = new Usuario_bodegaMng() { O_Usuario_bodega = new Usuario_bodega() { Id_usuario = itemUB.Id_usuario, Id_bodega = itemUB.Id_bodega } };
                    OUBMng.add(trans);
                }

                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
        }

        public static void usuarioUdt(Usuario oU)
        {
            IDbTransaction trans = null;
            try
            {
                trans = GenericDataAccess.BeginTransaction();
                UsuarioMng oUMng = new UsuarioMng();
                oUMng.O_Usuario = oU;
                oUMng.udt(trans);

                Usuario_bodega oUB = new Usuario_bodega() { Id_usuario = oU.Id };
                Usuario_bodegaMng OUBMng = new Usuario_bodegaMng() { O_Usuario_bodega = oUB };
                OUBMng.deleteByUsuario(trans);

                foreach (Usuario_bodega itemUB in oU.PLstUsuarioBodega)
                {
                    OUBMng = new Usuario_bodegaMng() { O_Usuario_bodega = new Usuario_bodega() { Id_usuario = itemUB.Id_usuario, Id_bodega = itemUB.Id_bodega } };
                    OUBMng.add(trans);
                }

                GenericDataAccess.CommitTransaction(trans);
            }
            catch
            {
                if (trans != null)
                    GenericDataAccess.RollbackTransaction(trans);
                throw;
            }
        }

        public static Usuario UsuarioCredencialesValidas(string email, string password)
        {
            Usuario o = new Usuario() { Email = email };
            try
            {
                
                UsuarioMng oMng = new UsuarioMng() { O_Usuario = o };
                oMng.selByEmail();
                if (string.Compare(password, o.Contrasenia) != 0)
                    o = new Usuario();
            }
            catch
            {

            }
            return o;
        }

        public static List<Usuario> UsuarioSelByRolAndBodega(enumRol rol, int id_bodega)
        {
            List<Usuario> lstBodega = new List<Usuario>();
            List<Usuario> lstRol = new List<Usuario>();
            try
            {
                UsuarioMng oUMng = new UsuarioMng();
                Usuario o = new Usuario() { Id_bodega = id_bodega };
                oUMng.O_Usuario = o;
                oUMng.fillLst();
                lstBodega = oUMng.Lst.FindAll(p => p.Id_bodega == id_bodega);

                string[] strRoles;

                foreach (Usuario itemUsr in lstBodega)
                {
                    strRoles = Roles.GetRolesForUser(itemUsr.Clave);
                    if (Array.Exists(strRoles, p => p.Equals(rol.ToString())))
                    {
                        itemUsr.Contrasenia = string.Empty;
                        lstRol.Add(itemUsr);
                    }
                }
            }
            catch
            {

                throw;
            }
            return lstRol;
        }

        #endregion

        #region Usuario bodega

        public static List<Usuario_bodega> usuarioBodegaFillByUsuario(int id_usuario)
        {
            List<Usuario_bodega> lst = new List<Usuario_bodega>();
            try
            {
                Usuario_bodegaMng oMng = new Usuario_bodegaMng() { O_Usuario_bodega = new Usuario_bodega() { Id_usuario = id_usuario } };
                oMng.fillLstByIdUsuario();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Transporte

        public static List<Transporte_tipo_transporte> TransporteGetByTipo(int id_tipo_transporte)
        {
            List<Transporte_tipo_transporte> lst = new List<Transporte_tipo_transporte>();
            try
            {
                Transporte_tipo_transporteMng oTTTMng = new Transporte_tipo_transporteMng() { O_Transporte_tipo_transporte = new Transporte_tipo_transporte() { Id_transporte_tipo = id_tipo_transporte } };
                oTTTMng.fillLstByTransporteTipo();
                lst = oTTTMng.Lst;
                //for (int i = 0; i < oTTTMng.Lst.Count; i++)
                //{
                //    Transporte o = new Transporte();
                //    Transporte_tipo_transporte ottt = oTTTMng.Lst[i];
                //    o.Id = ottt.Id_transporte;
                //    o.Nombre = ottt.Transporte;
                //    lst.Add(o);
                //}
            }
            catch
            {
                throw;
            }
            return lst;
        }

        public static Transporte_tipo Transporte_tipo_getyById(int id)
        {
            Transporte_tipoMng oTTMng = new Transporte_tipoMng();
            Transporte_tipo oTT = new Transporte_tipo();
            oTT.Id = id;
            oTTMng.O_Transporte_tipo = oTT;
            
            try
            {
                oTTMng.selById();
            }
            catch
            {
                
                throw;
            }
            return oTT;
        }

        public static List<Transporte_tipo> TransporteTipoGet()
        {
            List<Transporte_tipo> lst = new List<Transporte_tipo>();
            try
            {
                Transporte_tipoMng oMng = new Transporte_tipoMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }
    

        #endregion

        #region Tarima Almacen

        #region Tarima almacen proveedor 

        public static List<Tarima_almacen_proveedor> TarimaAlmacenProveedorFill()
        {
            List<Tarima_almacen_proveedor> lst = new List<Tarima_almacen_proveedor>();
            try
            {
                Tarima_almacen_proveedorMng oMng = new Tarima_almacen_proveedorMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #endregion

        public static string ToCSV(List<Object> lst)
        {
            string Csv = "[";

            if (lst.Count > 0)
            {
                Csv = "[";
                foreach (object o in lst)
                {
                    Csv += o.ToString() + ",";
                }

                Csv = Csv.Substring(0, Csv.Length - 1);
            }

            return Csv + "]";
        }

        #region Vigilancia
        public static List<Vigilante> vigilanteGetByBodega(int id_bodega)
        {
            List<Vigilante> lst = new List<Vigilante>();
            try
            {
                VigilanteMng oMng = new VigilanteMng() { O_Vigilante = new Vigilante() { Id_bodega = id_bodega } };
                oMng.fillLstByBodega();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Servicios

        public static List<Servicio> ServicioLst()
        {
            List<Servicio> lst = new List<Servicio>();
            try
            {
                ServicioMng oMng = new ServicioMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region Etiqueta tipo

        public static List<Etiqueta_tipo> etiquetaTipoGet()
        {
            List<Etiqueta_tipo> lst = new List<Etiqueta_tipo>();
            try
            {
                Etiqueta_tipoMng oMng = new Etiqueta_tipoMng();
                oMng.fillLst();
                lst = oMng.Lst;
            }
            catch
            {
                throw;
            }
            return lst;
        }

        #endregion
    }
}
