/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Intentor.Yamapper.Mapper;
using Intentor.Utilities;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Driver para acesso ao SQL Server.
    /// </summary>
    public class SqlServerDriver : IDataBaseDriver {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="parameters">Parâmetros de configuração do driver.</param>
        public SqlServerDriver(Dictionary<string, string> parameters) {
            _parameters = parameters;
        }

        #endregion

        #region Campos

        /// <summary>
        /// Parâmetros do driver.
        /// </summary>
        private Dictionary<string, string> _parameters;

        #endregion

        #region IDataBaseDriver Members

        public IDbConnection CreateConnection() {
            return new SqlConnection();
        }

        public IDbDataAdapter CreateDataAdapter() {
            return new SqlDataAdapter();
        }

        public string FormatNameForParameter(string parameterName) {
            return String.Concat("@", parameterName);
        }

        public string GetCommandForPrimaryKeyValue() {
            return "SELECT SCOPE_IDENTITY()";
        }

        /// <summary>
        /// Obtém o comando para limitação de registros em uma consulta.
        /// </summary>
        /// <param name="sql">Comandos SQL a serem executados.</param>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <remarks>
        /// O comando <c>LIMIT</c> do SQL Server possui a seguinte estrutura:
        /// <code>
        /// SELECT TOP {VALOR DO LIMIT} *
        ///	FROM
        ///		(SELECT (ROW_NUMBER() OVER({ORDER BY ORIGINAL}) AS RowNum
        ///		    , {COLUNAS DA TABELA}
        ///		FROM {TABELA ORIGINAL}
        ///		WHERE {WHERE ORIGINAL}) AS t
        ///		{original from}) as query
        ///	WHERE RowNum >= {VALOR DO OFFSET}
        /// </code>
        /// </remarks>
        /// <returns>Instrução SQL formatada.</returns>
        public string GetCommandForLimit(SqlSelectString sql, int offset, int limit) {
            if (offset > 0 || limit > 0) {
                StringBuilder modifiedSql = new StringBuilder();
                string[] fieldNames = sql.GetFieldNames();
                string orderBy = sql.OrderBy;
                if (orderBy.IsNullOrEmpty()) orderBy = " ORDER BY " + fieldNames[0];

                modifiedSql.AppendFormat("SELECT (ROW_NUMBER() OVER({0})) AS RowNum,{1} {2} {3}"
                    , orderBy
                    , String.Join(",", fieldNames)
                    , sql.From
                    , sql.Where);

                return String.Format(
                    @"SELECT TOP {2} * FROM ({0}) AS t WHERE RowNum > {1}"
                   , modifiedSql.ToString().Trim()
                   , offset
                   , limit);
            } else return sql.ToString();
        }

        public Dictionary<string, string> Parameters {
            get { return _parameters; }
        }

        public string ParameterPrefix {
            get { return "@"; }
        }

        #endregion
    }
}
