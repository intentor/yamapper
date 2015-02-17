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
using Intentor.Yamapper.Mapper;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Driver para acesso ao Oracle, utilizando System.Data.OracleClient.
    /// </summary>
    /// <remarks>
    /// Parâmetros:
    /// - assembly: full qualified name do assembly do provedor.
    /// </remarks>
    public class OracleDriver : ReflexionBasedDriver {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="parameters">Parâmetros de configuração do driver.</param>
        public OracleDriver(Dictionary<string, string> parameters) {
            _parameters = parameters;

            #region Verificação de parâmetros

            string assembly = "System.Data.OracleClient, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            if (_parameters.ContainsKey("assembly")) assembly = _parameters["assembly"];

            #endregion

            this.ConfigureDriver(assembly
                , "System.Data.OracleClient.OracleConnection"
                , "System.Data.OracleClient.OracleDataAdapter"
                , ":"
                , "SELECT <SequenceName>.NEXTVAL FROM DUAL");
        }

        /// <summary>
        /// Obtém o comando para limitação de registros em uma consulta.
        /// </summary>
        /// <param name="sql">Comandos SQL a serem executados.</param>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <returns>Instrução SQL formatada.</returns>
        public override string GetCommandForLimit(SqlSelectString sql, int offset, int limit) {
            if (offset > 0 || limit > 0) {
                StringBuilder modifiedSql = new StringBuilder();

                if (offset > 0) modifiedSql.Append("SELECT * FROM (SELECT T.*, ROWNUM NUM FROM (");
                else modifiedSql.Append("SELECT * FROM (");

                modifiedSql.Append(sql.ToString());

                if (offset > 0) modifiedSql.AppendFormat(") T) WHERE NUM > {0} AND NUM <= {1}", offset, offset + limit);
                else modifiedSql.AppendFormat(") WHERE ROWNUM <= {0}", limit);

                return modifiedSql.ToString();
            } else return sql.ToString();
        }
    }
}
