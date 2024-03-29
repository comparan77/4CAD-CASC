﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelCasc.catalog
{
    public class Aduana
    {
        #region Campos
        protected int _id;
        protected string _codigo;
        protected string _nombre;
        protected bool _IsActive;
        #endregion

        #region Propiedades
        public int Id { get { return _id; } set { _id = value; } }
        public string Codigo { get { return _codigo; } set { _codigo = value; } }
        public string Nombre { get { return _nombre; } set { _nombre = value; } }
        public bool IsActive { get { return _IsActive; } set { _IsActive = value; } }
        #endregion

        #region Constructores
        public Aduana()
        {
            this._codigo = String.Empty;
            this._nombre = String.Empty;
            this._IsActive = false;
        }
        #endregion
    }
}
