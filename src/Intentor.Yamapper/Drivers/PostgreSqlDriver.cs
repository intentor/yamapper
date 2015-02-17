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
using Intentor.Utilities;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Driver para acesso ao PostgreSQL, utilizando Npgsql.
    /// </summary>
    /// <remarks>
    /// Parâmetros:
    /// - assembly: full qualified name do assembly do provedor.
    /// </remarks>
    public class PostgreSqlDriver : ReflexionBasedDriver {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="parameters">Parâmetros de configuração do driver.</param>
        public PostgreSqlDriver(Dictionary<string, string> parameters) {
            _parameters = parameters;

            #region Verificação de parâmetros

            string assembly = "Npgsql, Version=2.0.11.92, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7";
            if (_parameters.ContainsKey("assembly")) assembly = _parameters["assembly"];

            #endregion

            this.ConfigureDriver(assembly
                , "Npgsql.NpgsqlConnection"
                , "Npgsql.NpgsqlDataAdapter"
                , ":"
                , "SELECT nextval('<SequenceName>'::regclass);");
        }

        /// <summary>
        /// Obtém o comando para limitação de registros em uma consulta.
        /// </summary>
        /// <param name="sql">Comandos SQL a serem executados.</param>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <returns>Instrução SQL formatada.</returns>
        public override string GetCommandForLimit(SqlSelectString sql, int offset, int limit) {
            StringBuilder modifiedSql = new StringBuilder(sql.ToString());
            if (offset > 0 || limit > 0) {
                if (offset > 0 && limit > 0) modifiedSql.AppendFormat(" LIMIT {0} OFFSET {1}", limit, offset);
                else if (offset > 0 && limit == 0) modifiedSql.AppendFormat(" OFFSET {0}", offset);
                else if (offset == 0 && limit > 0) modifiedSql.AppendFormat(" LIMIT {0}", limit);
            }

            return modifiedSql.ToString();
        }
    }
}
