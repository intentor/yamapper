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
    /// Driver para acesso ao MySql, utilizando MySqlClient.
    /// </summary>
    /// <remarks>
    /// Parâmetros:
    /// - assembly: full qualified name do assembly do provedor.
    /// - defaultLimit: valor padrão do limite de obtenção de dados (numérico).
    /// </remarks>
    public class MySqlDriver : ReflexionBasedDriver {
        /// <summary>
        /// Valor padrão do Limit do utilizado para paginação.
        /// </summary>
        private int defaultLimit = 100;

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="parameters">Parâmetros de configuração do driver.</param>
        public MySqlDriver(Dictionary<string, string> parameters) {
            _parameters = parameters;

            #region Verificação de parâmetros

            string assembly = "MySql.Data, Version=6.3.5.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d";
            if (_parameters.ContainsKey("assembly")) assembly = _parameters["assembly"];
            if (_parameters.ContainsKey("defaultLimit")) this.defaultLimit = _parameters["defaultLimit"].Parse<int>();

            #endregion

            this.ConfigureDriver(assembly
                , "MySql.Data.MySqlClient.MySqlConnection"
                , "MySql.Data.MySqlClient.MySqlDataAdapter"
                , "?"
                , "SELECT LAST_INSERT_ID()");
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
                modifiedSql.Append(" LIMIT ");
                if (offset > 0 && limit > 0) modifiedSql.AppendFormat("{0},{1}", offset, limit);
                else if (offset > 0 && limit == 0) modifiedSql.AppendFormat("{0},{1}", offset, this.defaultLimit);
                else modifiedSql.Append(limit);
            }

            return modifiedSql.ToString();
        }
    }
}
