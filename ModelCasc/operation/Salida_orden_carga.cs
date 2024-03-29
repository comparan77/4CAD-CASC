﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelCasc.operation
{
    public class Salida_orden_carga
    {
        #region Campos
        protected int _id;
        protected int _id_tipo_carga;
        protected int _id_usuario;
        protected int _id_salida_trafico;
        protected string _folio_orden_carga;
        protected bool _tiene_salida;
        protected string _observaciones_tranpsorte;
        #endregion

        #region Propiedades
        public int Id { get { return _id; } set { _id = value; } }
        public int Id_tipo_carga { get { return _id_tipo_carga; } set { _id_tipo_carga = value; } }
        public int Id_usuario { get { return _id_usuario; } set { _id_usuario = value; } }
        public int Id_salida_trafico { get { return _id_salida_trafico; } set { _id_salida_trafico = value; } }
        public string Folio_orden_carga { get { return _folio_orden_carga; } set { _folio_orden_carga = value; } }
        public bool Tiene_salida { get { return _tiene_salida; } set { _tiene_salida = value; } }
        public string Observaciones_tranpsorte { get { return _observaciones_tranpsorte; } set { _observaciones_tranpsorte = value; } }
        public List<Salida_orden_carga_rem> LstRem { get; set; }
        public Salida_trafico PSalidaTrafico { get; set; }
        public string TipoCarga { get; set; }
        public string TipoEnvio { get; set; }
        public List<Salida> LstSalida { get ; set; }
        public int Id_bodega_ubicacion { get; set; }
        public List<Salida_orden_carga_tc> PLstSalOCTransCond { get; set; }
        public string TransporteCondicionObs { get; set; }
        #endregion

        #region Constructores
        public Salida_orden_carga()
		{
			this._id_tipo_carga = 0;
			this._id_usuario = 0;
			this._id_salida_trafico = 0;
			this._folio_orden_carga = String.Empty;
            this._tiene_salida = false;
            this._observaciones_tranpsorte = string.Empty;
		}
        #endregion
    }
}
