/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Intentor.Yamapper.Drivers;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Define critérios para acesso ao banco de dados.
    /// </summary>
    public sealed class Criteria : ICloneable {
        #region Campos

        internal Dictionary<IExpression, LogicalOperator> _expressions = new Dictionary<IExpression, LogicalOperator>(0);
        internal List<Order> _orders = new List<Order>(0);
        internal List<ParameterData> _parameters = new List<ParameterData>(0);

        #endregion

        #region Propriedades

        /// <summary>
        /// Posição do registro inicial para obtenção dos registros.
        /// </summary>
        public int OffsetValue {
            get;
            set;
        }

        /// <summary>
        /// Quantidade máxima de registros a serem retornados.
        /// </summary>
        public int LimitValue {
            get;
            set;
        }

        #endregion

        #region Métodos

        #region Internal

        /// <summary>
        /// Obtém a cláusula Where.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade que contém os campos.</typeparam>
        /// <param name="parameterPrefix">Prefixo a ser utilizado nos parâmetros.</param>
        /// <returns> Retorna a cláusula Where.</returns>
        internal string GetWhereClause<T>(string parameterPrefix)
            where T : class {
            var sql = new StringBuilder();

            if (_expressions.Count > 0) {
                sql.Append(" WHERE ");
                bool isFirstExecution = true;
                foreach (KeyValuePair<IExpression, LogicalOperator> exp in _expressions) {
                    if (isFirstExecution) isFirstExecution = false;
                    else sql.AppendFormat(" {0} ", exp.Value.ToString().ToUpper());

                    sql.Append(exp.Key.ToSqlString<T>(parameterPrefix, _parameters));
                }
            }

            return sql.ToString();
        }

        /// <summary>
        /// Obtém a cláusula Order By.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade que contém os campos.</typeparam>
        /// <returns>Retorna a cláusula Order By.</returns>
        internal string GetOrderByClause<T>()
            where T : class {
            var sql = new StringBuilder();
            bool firstExecution = true;

            if (_orders.Count > 0) {
                sql.Append(" ORDER BY");
                foreach (Order ord in _orders) {
                    if (firstExecution) firstExecution = false;
                    else sql.Append(',');

                    string fieldName =
                        DbMapperHelper.Mappings[typeof(T).FullName].Fields.First(field => field.PropertyName.Equals(ord.PropertyName)).FieldName;

                    sql.Append(' ');
                    sql.Append(fieldName);

                    if (!ord.Ascending) sql.Append(" DESC");
                }
            }

            return sql.ToString();
        }

        /// <summary>
        /// Popula uma lista de parâmetros a partir de um determinado driver.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> a ser populado com os parâmetros.</param>
        internal void FillParameters(IDbCommand cmd) {
            foreach (ParameterData p in _parameters) {
                cmd.CreateParameter(p.Name, p.Value);
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Insere uma expressão ao critério de acesso ao banco de dados.
        /// </summary>
        /// <param name="expression">Objeto <see cref="IExpression"/> contendo os parâmetros da expressão.</param>
        /// <remarks>A expressão será adicionada com o operador lógico AND.</remarks>
        /// <returns>Objeto <see cref="Criteria"/> da instância atual.</returns>
        public Criteria Add(IExpression expression) {
            return this.Add(expression, LogicalOperator.And);
        }

        /// <summary>
        /// Insere uma expressão ao critério de acesso ao banco de dados.
        /// </summary>
        /// <param name="expression">Objeto <see cref="IExpression"/> contendo os parâmetros da expressão.</param>
        /// <param name="logicalOperator">Operador lógico da expressão adicionada.</param>
        /// <returns>Objeto <see cref="Criteria"/> da instância atual.</returns>
        public Criteria Add(IExpression expression, LogicalOperator logicalOperator) {
            _expressions.Add(expression, logicalOperator);

            return this;
        }

        /// <summary>
        /// Insere uma cláusula de ordenação ao critério de acesso ao banco de dados.
        /// </summary>
        /// <param name="order">Objeto <see cref="Order"/> contendo os parâmetros da ordenação.</param>
        /// <returns>Objeto <see cref="Criteria"/> da instância atual.</returns>
        public Criteria AddOrder(Order order) {
            _orders.Add(order);

            return this;
        }

        /// <summary>
        /// Insere uma cláusula de ordenação ao critério de acesso ao banco de dados.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <remarks>Ocorrerá ordenação pelo campo de forma ascendente.</remarks>
        /// <returns>Objeto <see cref="Criteria"/> da instância atual.</returns>
        public Criteria AddOrder(string propertyName) {
            _orders.Add(new Order(propertyName, true));

            return this;
        }

        /// <summary>
        /// Insere uma cláusula de ordenação ao critério de acesso ao banco de dados.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <param name="ascending">Identifica se se deve ordernar de forma ascendente ou descendente.</param>
        /// <returns>Objeto <see cref="Criteria"/> da instância atual.</returns>
        public Criteria AddOrder(string propertyName
            , bool ascending) {
            _orders.Add(new Order(propertyName, ascending));

            return this;
        }

        /// <summary>
        /// Define a posição do registro inicial para obtenção de dados, com o primeiro registro sendo 0.
        /// </summary>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <returns>Objeto <see cref="Criteria"/>.</returns>
        public Criteria Offset(int offset) {
            this.OffsetValue = offset;
            return this;
        }

        /// <summary>
        /// Define a quantidade máxima de registros a serem retornados.
        /// </summary>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <returns>Objeto <see cref="Criteria"/>.</returns>
        public Criteria Limit(int limit) {
            this.LimitValue = limit;
            return this;
        }

        /// <summary>
        /// Cria uma instância da classe.
        /// </summary>
        /// <returns>Objeto <see cref="Criteria"/>.</returns>
        public static Criteria Create() {
            return new Criteria();
        }

        /// <summary>
        /// Cria uma cópia do objeto.
        /// </summary>
        /// <returns>Objeto copiado.</returns>
        public object Clone() {
            Criteria crit = Criteria.Create();

            crit._expressions = new Dictionary<IExpression, LogicalOperator>(this._expressions.Count);
            foreach (var exp in this._expressions) crit._expressions.Add(exp.Key, exp.Value);

            crit._orders = new List<Order>(this._orders.Count);
            crit._orders.AddRange(this._orders);

            crit._parameters = new List<ParameterData>(this._parameters.Count);
            crit._parameters.AddRange(this._parameters);

            return crit;
        }

        #endregion

        #endregion
    }
}