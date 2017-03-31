﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ModelCasc.report.operation;

namespace ModelCasc.operation
{
    public class Salida_aud_uni : UsrActivity, IAuditoriaCAEApp, ICasc028
    {
        #region Campos
        protected int _id;
        protected int _id_salida_orden_carga;
        protected int _id_transporte_tipo;
        protected string _informa;
        protected string _referencia;
        protected string _operador;
        protected string _placa;
        protected string _caja;
        protected string _caja1;
        protected string _caja2;
        protected string _sello;
        protected string _acta_informativa;
        protected DateTime _fecha;
        protected string _vigilante;
        #endregion

        #region Propiedades
        public int Id { get { return _id; } set { _id = value; } }
        public int Id_salida_orden_carga { get { return _id_salida_orden_carga; } set { _id_salida_orden_carga = value; } }
        public int Id_transporte_tipo { get { return _id_transporte_tipo; } set { _id_transporte_tipo = value; } }
        public string Informa { get { return _informa; } set { _informa = value; } }
        public string Referencia { get { return _referencia; } set { _referencia = value; } }
        public string Operador { get { return _operador; } set { _operador = value; } }
        public string Placa { get { return _placa; } set { _placa = value; } }
        public string Caja { get { return _caja; } set { _caja = value; } }
        public string Caja1 { get { return _caja1; } set { _caja1 = value; } }
        public string Caja2 { get { return _caja2; } set { _caja2 = value; } }
        public string Sello { get { return _sello; } set { _sello = value; } }
        public string Acta_informativa { get { return _acta_informativa; } set { _acta_informativa = value; } }
        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public string Vigilante { get { return _vigilante; } set { _vigilante = value; } }
        public List<Salida_aud_uni_files> PLstSalAudUniFiles { get; set; }
        #endregion

        #region Constructores
        public Salida_aud_uni()
		{
			this._id_salida_orden_carga = 0;
			this._id_transporte_tipo = 0;
			this._informa = String.Empty;
			this._referencia = String.Empty;
			this._operador = String.Empty;
			this._placa = String.Empty;
			this._caja = String.Empty;
			this._caja1 = String.Empty;
			this._caja2 = String.Empty;
			this._sello = String.Empty;
			this._acta_informativa = String.Empty;
			this._vigilante = String.Empty;
		}
        #endregion

        [JsonIgnore()]
        public int Id_fk
        {
            get
            {
                return this._id_salida_orden_carga;
            }
            set
            {
                this._id_salida_orden_carga = value;
            }
        }
        [JsonIgnore()]
        public string Relato
        {
            get { return this.Acta_informativa; }
            set { this.Acta_informativa = value; }
        }
        [JsonIgnore()]
        public string Vigilancia
        {
            get { return this.Vigilante; }
            set { this.Vigilante = value; }
        }
        [JsonIgnore()]
        public string Notificado
        {
            get { return this.Operador; }
            set { this.Operador = value; }
        }
        [JsonIgnore()]
        public List<IAudImage> PLstAudImg
        {
            get
            {
                return PLstSalAudUniFiles.Cast<IAudImage>().ToList();
            }
            set
            {
                PLstSalAudUniFiles = value.Cast<Salida_aud_uni_files>().ToList();
            }
        }
        public string prefixImg
        {
            get { return "sal_aud_uni_"; }
        }
        [JsonIgnore()]
        public IAuditoriaCAECppMng Mng
        {
            get { return new Salida_aud_uniMng(); }
        }
        [JsonIgnore()]
        public IAuditoriaCAEApp PAudOperation
        {
            get { return this; }
        }
        [JsonIgnore()]
        public string Cliente
        {
            get;
            set;
        }
        [JsonIgnore()]
        public string Lugar
        {
            get;
            set;
        }
        [JsonIgnore()]
        public string Informado
        {
            get { return this.Operador; }
        }
        [JsonIgnore()]
        public string Testigo
        {
            get { return string.Empty; }
        }
    }
}
