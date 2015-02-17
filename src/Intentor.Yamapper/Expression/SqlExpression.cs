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

namespace Intentor.Yamapper {
    /// <summary>
    /// Expressão SQL.
    /// </summary>
    /// <remarks>Utilizar o curinga {param} para indicação do ponto de substituição do nome do parâmetro.</remarks>
    public class SqlExpression : IExpression {
        /// <summary>
        /// Expressão SQL.
        /// </summary>
        private string _sql;

        /// <summary>
        /// Valor do parâmetro na expressão SQL.
        /// </summary>
        private object _value;

        /// <summary>
        /// Cria uma expressão SQL.
        /// </summary>
        /// <param name="sql">Expressão SQL</param>
        public SqlExpression(string sql)
            : this(sql, null) { }

        /// <summary>
        /// Cria uma expressão SQL.
        /// </summary>
        /// <param name="sql">Expressão SQL.</param>
        /// <param name="value">Valor do parâmetro na expressão SQL</param>
        public SqlExpression(string sql, object value) {
            _sql = sql;
            _value = value;
        }

        public string ToSqlString<T>(string parameterPrefix, List<ParameterData> parameters) {
            if (_value != null) {
                string paramName = String.Concat("pCustom", parameters.Count);
                parameters.Add(new ParameterData(paramName, _value));
                _sql = _sql.Replace("{param}", String.Concat(parameterPrefix, paramName));
            }

            return _sql;
        }
    }
}
