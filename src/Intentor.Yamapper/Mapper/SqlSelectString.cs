/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intentor.Utilities;

namespace Intentor.Yamapper.Mapper {
    /// <summary>
    /// Representa uma instrução SQL SELECT.
    /// </summary>
    public class SqlSelectString {
        #region Campos

        private string _select;
        private string _from;
        private string _where;
        private string _orderBy;

        #endregion

        #region Construtor

        /// <summary>
        /// Cria uma representação de instrução SQL.
        /// </summary>
        /// <param name="select">Parte SELECT da instrução SQL.</param>
        public SqlSelectString(string select)
            : this(select, null, null, null) { }

        /// <summary>
        /// Cria uma representação de instrução SQL.
        /// </summary>
        /// <param name="select">Parte SELECT da instrução SQL.</param>
        /// <param name="from">Parte FROM da instrução SQL.</param>
        public SqlSelectString(string select
            , string from)
            : this(select, from, null, null) { }

        /// <summary>
        /// Cria uma representação de instrução SQL.
        /// </summary>
        /// <param name="select">Parte SELECT da instrução SQL.</param>
        /// <param name="where">Parte WHERE da instrução SQL.</param>
        /// <param name="orderBy">Parte ORDER BY da instrução SQL.</param>
        public SqlSelectString(string select
            , string where
            , string orderBy)
            : this(select, null, where, orderBy) { }

        /// <summary>
        /// Cria uma representação de instrução SQL.
        /// </summary>
        /// <param name="select">Parte SELECT da instrução SQL.</param>
        /// <param name="from">Parte FROM da instrução SQL.</param>
        /// <param name="where">Parte WHERE da instrução SQL.</param>
        /// <param name="orderBy">Parte ORDER BY da instrução SQL.</param>
        public SqlSelectString(string select
            , string from
            , string where
            , string orderBy) {
            this.From = from;
            this.Select = select;
            this.Where = where;
            this.OrderBy = orderBy;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Parte SELECT da instrução SQL.
        /// </summary>
        public string Select {
            get { return _select; }
            set {
                int fromIndex = value.IndexOf("FROM");
                if (fromIndex >= 0) {
                    _select = value.Substring(0, fromIndex).Trim();
                    _from = value.Substring(fromIndex).Trim();
                } else _select = value;
            }
        }

        /// <summary>
        /// Parte FROM da instrução SQL.
        /// </summary>
        public string From {
            get { return _from; }
            set { _from = (value.IsNullOrEmpty() ? null : value.Trim()); }
        }

        /// <summary>
        /// Parte WHERE da instrução SQL.
        /// </summary>
        public string Where {
            get { return _where; }
            set { _where = (value.IsNullOrEmpty() ? null : value.Trim()); }
        }

        /// <summary>
        /// Parte ORDER BY da instrução SQL.
        /// </summary>
        public string OrderBy {
            get { return _orderBy; }
            set { _orderBy = (value.IsNullOrEmpty() ? null : value.Trim()); }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtém o nome dos campos.
        /// </summary>
        /// <returns>Campos obtidos.</returns>
        public string[] GetFieldNames() {
            //Começa em 6 pois é a palavra "SELECT ".
            string[] fields = _select.Substring(6).Split(',');
            return fields;
        }

        /// <summary>
        /// Instrução SQL representada pelo objeto.
        /// </summary>
        /// <returns>Instrução SQL montada.</returns>
        public override string ToString() {
            return String.Format("{0} {1} {2} {3}"
                , this.Select
                , this.From
                , this.Where
                , this.OrderBy).Trim();
        }

        #endregion
    }
}
