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
    /// Representa uma expressão de acesso ao banco de dados.
    /// </summary>
    public class SimpleExpression : IExpression {
        #region Campos

        protected string _propertyName;
        protected object _value1;
        protected object _value2;
        protected CommonOperator _operatorType;

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade.</param>
        /// <param name="operatorType">Operador da expressão.</param>
        public SimpleExpression(string propertyName
            , CommonOperator operatorType)
            : this(propertyName
                , operatorType
                , null
                , null) { }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade.</param>
        /// <param name="operatorType">Operador da expressão.</param>
        /// <param name="value">Valor da expressão.</param>
        public SimpleExpression(string propertyName
            , CommonOperator operatorType
            , object value)
            : this(propertyName
                , operatorType
                , value
                , null) { }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade.</param>
        /// <param name="operatorType">Operador da expressão.</param>
        /// <param name="value1">Valor 1 da expressão.</param>
        /// <param name="value2">Valor 2 da expressão.</param>
        public SimpleExpression(string propertyName
            , CommonOperator operatorType
            , object value1
            , object value2) {
            _propertyName = propertyName;
            _operatorType = operatorType;
            _value1 = value1;
            _value2 = value2;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtém o formato string de um operador.
        /// </summary>
        /// <param name="expressionOperator">Objeto <see cref="CommonOperator"/> que representa o operador.</param>
        /// <returns>Operador em formato string.</returns>
        private string GetOperator(CommonOperator expressionOperator) {
            string strOper = String.Empty;

            switch (expressionOperator) {
                case CommonOperator.Equal:
                    strOper = "=";
                    break;

                case CommonOperator.NotEqual:
                    strOper = "!=";
                    break;

                case CommonOperator.GreaterThan:
                    strOper = ">";
                    break;

                case CommonOperator.GreaterOrEqual:
                    strOper = ">=";
                    break;

                case CommonOperator.LesserThan:
                    strOper = "<";
                    break;

                case CommonOperator.LesserOrEqual:
                    strOper = "<=";
                    break;

                case CommonOperator.IsNull:
                    strOper = "IS NULL";
                    break;

                case CommonOperator.IsNotNull:
                    strOper = "IS NOT NULL";
                    break;

                case CommonOperator.Like:
                    strOper = "LIKE";
                    break;

                case CommonOperator.NotLike:
                    strOper = "NOT LIKE";
                    break;

                case CommonOperator.In:
                    strOper = "IN";
                    break;

                case CommonOperator.NotIn:
                    strOper = "NOT IN";
                    break;

                case CommonOperator.Between:
                    strOper = "BETWEEN";
                    break;
            }

            return strOper;
        }

        /// <summary>
        /// Obtém o nome de um parâmetro.
        /// </summary>
        /// <param name="propertyName">Nome do parâmetro.</param>
        /// <param name="parameters">Lista de parâmetros já adicionados.</param>
        /// <returns>Nome do parâmetro.</returns>
        private string GetParameterName(string propertyName, List<ParameterData> parameters) {
            return String.Concat("p", _propertyName, parameters.Count);
        }

        public string ToSqlString<T>(string parameterPrefix, List<ParameterData> parameters) {
            StringBuilder sql = new StringBuilder();
            string fieldName = DbMapperHelper.Mappings[typeof(T).FullName].Fields.First(field => field.PropertyName.Equals(_propertyName)).FieldName;

            sql.AppendFormat(" {0} {1} "
                , fieldName
                , this.GetOperator(_operatorType));

            switch (_operatorType) {
                case CommonOperator.IsNull:
                case CommonOperator.IsNotNull:
                    break;

                case CommonOperator.Like: {
                        string paramName = this.GetParameterName(_propertyName, parameters);
                        sql.Append(String.Concat(parameterPrefix, paramName));
                        parameters.Add(new ParameterData(paramName, String.Concat("%", _value1, "%")));
                    }
                    break;

                case CommonOperator.Between: {
                        string param1Name = this.GetParameterName(_propertyName, parameters);
                        parameters.Add(new ParameterData(param1Name, _value1));
                        string param2Name = this.GetParameterName(_propertyName, parameters);
                        parameters.Add(new ParameterData(param2Name, _value2));

                        sql.Append(String.Concat(parameterPrefix, param1Name));
                        sql.Append(" AND ");
                        sql.Append(String.Concat(parameterPrefix, param2Name));
                    }
                    break;

                case CommonOperator.In:
                case CommonOperator.NotIn: {
                        if (_value1 is Array) {
                            sql.Append("(");
                            object[] values = (object[])_value1;

                            for (int i = 0; i < values.Length; i++) {
                                if (i > 0) sql.Append(",");

                                string paramName = this.GetParameterName(_propertyName, parameters);
                                sql.Append(String.Concat(parameterPrefix, paramName));
                                parameters.Add(new ParameterData(paramName, values[i]));
                            }

                            sql.Append(")");
                        } else throw new ArgumentException(Messages.NotArrayOperatorIn);
                    }
                    break;

                default: {
                        string paramName = this.GetParameterName(_propertyName, parameters);
                        sql.Append(String.Concat(parameterPrefix, paramName));
                        parameters.Add(new ParameterData(paramName, _value1));
                    }
                    break;
            }

            sql.Append(" ");

            return sql.ToString();
        }

        #endregion
    }
}
