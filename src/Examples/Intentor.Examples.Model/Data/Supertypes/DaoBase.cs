/*********************************************
Generated by Intentor.Yamapper Class Generator
http://intentor.com.br/projects/yamapper/
*********************************************/
using System;
using System.Collections.Generic;
using Intentor.Yamapper;

namespace Intentor.Examples.Model.Data.Supertypes
{
    /// <summary>Supertype para Data Access Objects (Dao).</summary>
    public abstract class DaoBase
    {
        #region Campos

        /// <summary>Provedor de acesso ao banco de dados.</summary>
        protected DbProvider _provider;

        #endregion

        #region Propriedades

        /// <summary>Provedor de acesso ao banco de dados.</summary>
        public DbProvider CurrentProvider
        {
            get { return _provider; }
        }

        #endregion

        #region Métodos

        /// <summary>Obtém o provedor de acesso utilizado pela Dao.</summary>
        /// <remarks>Obtém o provedor padrão de acesso ao banco de dados.</remarks>
        protected void GetCurrentProvider()
        {
            _provider = DbProviderFactory.GetProvider();
        }

        /// <summary>Obtém o provedor de acesso utilizado pela Dao.</summary>
        /// <param name="connectionName">Nome da conexão para acesso ao banco de dados.</param>
        protected void GetCurrentProvider(string providerName)
        {
            _provider = DbProviderFactory.GetProvider(providerName);
        }

        /// <summary>Obtém o provedor de acesso utilizado pela Dao.</summary>
        /// <typeparam name="T">Tipo da entidade a ter o nome da conexão obtida.</typeparam>
        protected void GetCurrentProvider<T>()
            where T : class
        {
            _provider = DbProviderFactory.GetProvider<T>();
        }

        #endregion
    }
}
