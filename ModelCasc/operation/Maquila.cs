﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelCasc.operation
{
    public class Maquila
    {
        #region Campos
        protected int _id;
        protected int _id_ord_tbj_srv;
        protected DateTime _fecha;
        protected int _piezas;
        protected bool _capturada;
        #endregion

        #region Propiedades
        public int Id { get { return _id; } set { _id = value; } }
        public int Id_ord_tbj_srv { get { return _id_ord_tbj_srv; } set { _id_ord_tbj_srv = value; } }
        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public int Piezas { get { return _piezas; } set { _piezas = value; } }
        public bool Capturada { get { return _capturada; } set { _capturada = value; } }
        public List<Maquila_paso> PLstPasos { get; set; }
        #endregion

        #region Constructores
        public Maquila()
        {
            this._id_ord_tbj_srv = 0;
            this._fecha = default(DateTime);
            this._piezas = 0;
            this._capturada = false;
        }
        #endregion
    }
}
