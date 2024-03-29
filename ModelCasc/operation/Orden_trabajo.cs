﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelCasc.operation.liverpool;

namespace ModelCasc.operation
{
    [Serializable]
    public class Orden_trabajo
    {
        #region Campos
        protected int _id;
        protected string _folio;
        protected string _referencia;
        protected string _referencia_entrada;
        protected DateTime _fecha;
        protected bool _cerrada;
        protected string _supervisor;

        #endregion

        #region Propiedades
        public int Id { get { return _id; } set { _id = value; } }
        public string Folio { get { return _folio; } set { _folio = value; } }
        public string Referencia { get { return _referencia; } set { _referencia = value; } }
        public string Referencia_entrada { get { return _referencia_entrada; } set { _referencia_entrada = value; } }
        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public bool Cerrada { get { return _cerrada; } set { _cerrada = value; } }
        public List<Orden_trabajo_servicio> PLstOTSer { get; set; }
        public int Servicios { get; set; }
        public string Supervisor { get { return _supervisor; } set { _supervisor = value; } }
        public Entrada PEnt { get; set;}
        #endregion

        #region Constructores
        public Orden_trabajo()
		{
			this._folio = String.Empty;
            this._referencia = String.Empty;
            this._referencia_entrada = String.Empty;
            this._cerrada = false;
            this._supervisor = string.Empty;
		}
        #endregion
    }
}
