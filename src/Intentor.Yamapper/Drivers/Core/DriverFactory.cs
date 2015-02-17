/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Intentor.Utilities;
using Intentor.Yamapper.Drivers;
using Intentor.Yamapper.Mapper;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Fábrica de drivers de acesso ao banco de dados.
    /// </summary>
    public sealed class DriverFactory {
        #region Construtor

        /// <summary>
        /// Constutor da classe.
        /// </summary>
        /// <param name="provider">String que representa o provider da conexão.</param>
        public DriverFactory(string providerName)
            : this(providerName, null) {
        }

        /// <summary>
        /// Constutor da classe.
        /// </summary>
        /// <param name="providerName">String que representa o provider da conexão.</param>
        /// <param name="parameters">Parâmetros a serem passados ao provedor.</param>
        public DriverFactory(string providerName, Dictionary<string, string> parameters) {
            _providerName = providerName;
            if (parameters.IsNullOrDbNull()) parameters = new Dictionary<string, string>();
            _driver = (IDataBaseDriver)ReflectionHelper.CreateInstance(providerName, "Intentor.Yamapper", parameters);
        }

        #endregion

        #region Campos

        /// <summary>
        /// Provider da conexão.
        /// </summary>
        private string _providerName;

        /// <summary>
        /// Driver para resolução de objetos para o provedor atual.
        /// </summary>
        private IDataBaseDriver _driver;

        #endregion

        #region Propriedades

        /// <summary>
        /// Provider da conexão.
        /// </summary>
        public string Provider {
            get { return _providerName; }
        }

        /// <summary>
        /// Caractere utilizado como prefixo para criação de parâmetros nas instruções SQL.
        /// </summary
        public string ParameterPrefix {
            get { return _driver.ParameterPrefix; }
        }

        #endregion

        #region Métodos

        #region Public

        /// <summary>
        /// Obtém o objeto <see cref="IDbConnection"/> baseado no provider especificado.
        /// </summary>
        /// <param name="connectionString">String de conexão utilizada para acesso ao banco de dados.</param>
        /// <returns>Objeto <see cref="IDbConnection"/> instanciado com o tipo correto a partir do provider especifiado.</returns>
        public IDbConnection CreateConnection(string connectionString) {
            IDbConnection db = _driver.CreateConnection();
            db.ConnectionString = connectionString;

            return db;
        }

        /// <summary>
        /// Obtém o objeto <see cref="IDbDataAdapter"/> baseado no provider especificado.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> a ser utilizado como base para criação do objeto <see cref="IDbDataAdapter"/>.</param>
        /// <returns>Objeto <see cref="IDbDataAdapter"/> instanciado com o tipo correto a partir do provider especifiado.</returns>
        public IDbDataAdapter CreateDataAdapter(IDbCommand cmd) {
            IDbDataAdapter da = _driver.CreateDataAdapter();
            da.SelectCommand = cmd;

            return da;
        }

        /// <summary>
        /// Formata um nome de parâmetro de acordo com o prefixo definido.
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser formatado.</param>
        /// <returns>Nome do parâmetro formatado.</returns>
        public string FormatNameForParameter(string parameterName) {
            return _driver.FormatNameForParameter(parameterName);
        }

        /// <summary>
        /// Comando utilizado para obtenção da primary key de um objeto após sua inserção.
        /// </summary>
        public string GetCommandForPrimaryKeyValue() {
            return _driver.GetCommandForPrimaryKeyValue();
        }

        /// <summary>
        /// Obtém o comando para limitação de registros em uma consulta.
        /// </summary>
        /// <param name="sql">Comandos SQL a serem executados.</param>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <returns></returns>
        public string GetCommandForLimit(SqlSelectString sql, int offset, int limit) {
            return _driver.GetCommandForLimit(sql, offset, limit);
        }

        #endregion

        #endregion
    }
}
