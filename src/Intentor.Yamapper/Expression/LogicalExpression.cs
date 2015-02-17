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
    /// Representa uma expressão lógica.
    /// </summary>
    public abstract class LogicalExpression : IExpression {
        #region Campos

        /// <summary>
        /// Expressões da expressão lógica.
        /// </summary>
        protected IExpression[] _expressions;

        #endregion

        #region Construtor

        /// <summary>
        /// Cria uma nova expressão lógica.
        /// </summary>
        /// <param name="expressions">Expressões da expressão lógica.</param>
        public LogicalExpression(params IExpression[] expressions) {
            _expressions = expressions;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Operador da expressão.
        /// </summary>
        protected abstract string Operator { get; }

        #endregion

        #region Métodos

        public string ToSqlString<T>(string parameterPrefix, List<ParameterData> parameters) {
            StringBuilder sb = new StringBuilder();
            bool isFirstExecution = true;

            sb.Append("(");
            foreach (IExpression expression in _expressions) {
                if (isFirstExecution) isFirstExecution = false;
                else sb.AppendFormat("{0}", this.Operator);

                sb.Append(expression.ToSqlString<T>(parameterPrefix, parameters));
            }
            sb.AppendFormat(")");

            return sb.ToString();
        }

        #endregion
    }
}
