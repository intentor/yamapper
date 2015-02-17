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
using Intentor.Yamapper.Mapper;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Driver abstrato para representação de um provedor de acesso a um banco de dados.
    /// </summary>
    public abstract class ReflexionBasedDriver :
        IDataBaseDriver {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        protected ReflexionBasedDriver() {
        }

        #endregion

        #region Campos

        /// <summary>
        /// Parâmetros do driver.
        /// </summary>
        protected Dictionary<string, string> _parameters;

        /// <summary>
        /// Nome do assembly do provedor, incluindo ninformações de cultura, versão e token público.
        /// </summary>
        private string _assemblyName;

        /// <summary>
        /// Tipo do objeto <see cref="IDbConnection"/>.
        /// </summary>
        private Type _connectionType;

        /// <summary>
        /// Tipo do objeto <see cref="IDbDataAdapter"/>.
        /// </summary>
        private Type _dataAdapterType;

        /// <summary>
        /// Caractere utilizado como prefixo para criação de parâmetros nas instruções SQL.
        /// </summary>
        private string _parameterPrefix;

        /// <summary>
        /// Comando utilizado para obtenção de dados em campos de autonumeração.
        /// </summary>
        private string _commandForAutoIncrement;

        #endregion

        #region Propriedades

        public Dictionary<string, string> Parameters {
            get { return _parameters; }
        }

        public string ParameterPrefix {
            get { return _parameterPrefix; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Configura o driver.
        /// </summary>
        /// <param name="assemblyName">Nome do assembly do provedor, incluindo informações de cultura, versão e token público.</param>
        /// <param name="connectionType">Tipo do objeto <see cref="IDbConnection"/>.</param>
        /// <param name="dataAdapterType">Tipo do objeto <see cref="IDbDataAdapter"/></param>
        /// <param name="parameterPrefix">Caractere utilizado como prefixo para criação de parâmetros nas instruções SQL.</param>
        /// <param name="commandForAutoIncrement">Comando utilizado para obtenção de dados em campos de autonumeração.</param>
        protected void ConfigureDriver(string assemblyName
            , string connectionType
            , string dataAdapterType
            , string parameterPrefix
            , string commandForAutoIncrement) {
            _assemblyName = assemblyName;
            _connectionType = ReflectionHelper.GetTypeFromAssembly(connectionType, _assemblyName);
            _dataAdapterType = ReflectionHelper.GetTypeFromAssembly(dataAdapterType, _assemblyName);
            _parameterPrefix = parameterPrefix;
            _commandForAutoIncrement = commandForAutoIncrement;
        }

        public IDbConnection CreateConnection() {
            return (IDbConnection)Activator.CreateInstance(_connectionType);
        }

        public IDbDataAdapter CreateDataAdapter() {
            return (IDbDataAdapter)Activator.CreateInstance(_dataAdapterType);
        }

        public string FormatNameForParameter(string parameterName) {
            return String.Concat(_parameterPrefix, parameterName);
        }

        public string GetCommandForPrimaryKeyValue() {
            return _commandForAutoIncrement;
        }

        public abstract string GetCommandForLimit(SqlSelectString sql, int offset, int limit);

        #endregion
    }
}
