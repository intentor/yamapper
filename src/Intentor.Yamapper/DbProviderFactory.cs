/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Web;
using Intentor.Utilities;
using Intentor.Yamapper.Drivers;

namespace Intentor.Yamapper {
    /// <summary>
    /// Gerencia a obtenção de objetos <see cref="DbProvider"/>.
    /// </summary>
    public static class DbProviderFactory {
        #region Métodos

        #region Private

        /// <summary>
        /// Cria um provedor baseado em dados de uma conexão.
        /// </summary>
        /// <param name="conn">Dados da conexão.</param>
        /// <returns>Provedor de acesso ao banco de dados.</returns>
        private static DbProvider CreateProvider(ConnectionInfo conn) {
            //Instancia o driver factory correto.
            DriverFactory driver = new DriverFactory(conn.ProviderName, conn.Parameters);

            //Cria o objeto de conexão mas não abre sua conexão.
            DbProvider cn = new DbProvider(conn.ConnectionString, driver);

            return cn;
        }

        /// <summary>
        /// Obtém um provedor com base em seu nome.
        /// </summary>
        /// <param name="providerName">Nome da string de conexão do banco de dados definida no arquivo web.config.</param>
        /// <returns>Retorna um objeto <see cref="DbProvider"/> que representa a conexão com o banco de dados solicitado.</returns>
        private static DbProvider ResolveProvider(string connectionName) {
            DbProvider provider;

            try {
                provider = (DbProvider)HttpContext.Current.Items[connectionName];
            } catch {
                provider = null;
            }

            if (provider.IsNullOrDbNull() || provider.Disposed) {
                var connections = YamapperConfigurations.DatabaseConnections;
                if (connections == null) {
                    throw new DatabaseConnectionException(Messages.DatabaseConnectionsEmpty);
                } else {
                    foreach (ConnectionInfo conn in connections) {
                        if (conn.Name.Equals(connectionName)) {
                            provider = CreateProvider(conn);
                            try {
                                HttpContext.Current.Items[connectionName] = provider;
                            } catch { }
                            break;
                        }
                    }
                }
            }

            if (provider == null) throw new DatabaseConnectionException(String.Format(Messages.DatabaseConnectionNotFound, connectionName));
            if (!provider.IsConnectionOpen) provider.OpenConnection();

            return provider;
        }

        #endregion

        #region Public

        /// <summary>
        /// Cria mapeamento das entidades de banco de dados.
        /// </summary>
        public static void BuildMappings() {
            //Verifica se é para criar e se já foi realizado o mapeamento das entidades.
            if (!DbMapperHelper.IsMapped) DbMapperHelper.BuildMappings();
        }

        /// <summary>
        /// Cria objetos de conexão para todas as strings de conexão disponíveis no web.config.
        /// </summary>
        public static void CreateProviders() {
            //Verifica se é para criar e se já foi realizado o mapeamento das entidades.
            if (!DbMapperHelper.IsMapped) DbMapperHelper.BuildMappings();

            //Cria os providers.
            if (HttpContext.Current != null) {
                foreach (ConnectionInfo conn in YamapperConfigurations.DatabaseConnections)
                    if (!HttpContext.Current.Items.Contains(conn.Name))
                        HttpContext.Current.Items[conn.Name] = CreateProvider(conn);
            }
        }

        /// <summary>
        /// Descarta todas as conexões existentes.
        /// </summary>
        /// <remarks>Durante o descarte, as conexões são fechadas.</remarks>
        public static void DisposeProviders() {
            if (HttpContext.Current != null) {
                foreach (ConnectionInfo conn in YamapperConfigurations.DatabaseConnections) {
                    try {
                        if (HttpContext.Current.Items.Contains(conn.Name))
                            ((DbProvider)HttpContext.Current.Items[conn.Name]).Dispose();
                    } catch { }
                }
            }
        }

        /// <summary>
        /// Obtém o provedor padrão da aplicação, se houver.
        /// </summary>
        /// <returns>Retorna um objeto <see cref="DbProvider"/> que representa a conexão com o banco de dados solicitado.
        /// </returns>
        public static DbProvider GetProvider() {
            //Verifica se é para criar e se já foi realizado o mapeamento das entidades.
            if (!DbMapperHelper.IsMapped) DbMapperHelper.BuildMappings();

            //Obtém o provedor padrão.
            string connectionName = YamapperConfigurations.DefaultConnection;

            Check.Require(!String.IsNullOrEmpty(connectionName),
                "Provider padrão não definido.");

            return ResolveProvider(connectionName);
        }

        /// <summary>
        /// Obtém o provedor para um determinado nome de string de conexão.
        /// </summary>
        /// <param name="connectionName">Nome da conexão para acesso ao banco de dados.</param>
        /// <returns>Retorna um objeto <see cref="DbProvider"/> que representa a conexão com o banco de dados solicitado.</returns>
        public static DbProvider GetProvider(string connectionName) {
            //Verifica se é para criar e se já foi realizado o mapeamento das entidades.
            if (!DbMapperHelper.IsMapped) DbMapperHelper.BuildMappings();

            DbProvider provider;

            if (connectionName.IsNullOrEmpty())
                provider = GetProvider();
            else
                provider = ResolveProvider(connectionName);

            return provider;
        }

        /// <summary>
        /// Obtém o provedor para um determinado nome de string de conexão.
        /// </summary>
        /// <typeparam name="T">Objeto <see cref="MappingEntity"/> para extração da conexão.</typeparam>
        /// <remarks>Caso o objeto não possua uma conexão definida a conexão padrão será utilizada.</remarks>
        /// <returns>Retorna um objeto <see cref="DbProvider"/> que representa a conexãoo com o banco de dados solicitado.</returns>
        public static DbProvider GetProvider<T>()
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            //Verifica se é para criar e se já foi realizado o mapeamento das entidades.
            if (!DbMapperHelper.IsMapped) DbMapperHelper.BuildMappings();

            DbProvider provider;

            string connectionName = DbMapperHelper.Mappings[typeof(T).FullName].ConnectionName;

            if (connectionName.IsNullOrEmpty())
                provider = GetProvider();
            else
                provider = ResolveProvider(connectionName);

            return provider;
        }

        #endregion

        #endregion
    }
}
