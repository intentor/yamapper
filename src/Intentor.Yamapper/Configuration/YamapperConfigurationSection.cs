/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Grupo de seção de configuração do componente ConnectionManager.
    /// </summary>
    public class YamapperConfigurationSection
        : ConfigurationSection
        , IYamapperConfigurationData {
        /// <summary>
        /// Identifica a conexão padrão para acesso à base de dados.
        /// </summary>
        [ConfigurationProperty("defaultConnection", IsRequired = false)]
        public string DefaultConnection {
            get {
                return this["defaultConnection"].ToString();
            }
            set {
                this["defaultConnection"] = value;
            }
        }

        /// <summary>
        /// Identifica a localização do arquivo de configurações do componente.
        /// </summary>
        [ConfigurationProperty("pathConfigurationFile", IsRequired = false)]
        public string PathConfigurationFile {
            get {
                return this["pathConfigurationFile"].ToString();
            }
            set {
                this["pathConfigurationFile"] = value;
            }
        }

        /// <summary>
        /// Identifica o assembly que contém as entidades a serem mapeadas.
        /// </summary>
        [ConfigurationProperty("mappingAssembly", IsRequired = false)]
        public string MappingAssembly {
            get {
                return this["mappingAssembly"].ToString();
            }
            set {
                this["mappingAssembly"] = value;
            }
        }

        /// <summary>
        /// Identifica o diretório que contém os arquivos XML de mapeamento das tabelas.
        /// </summary>
        [ConfigurationProperty("mappingsFolder", IsRequired = false)]
        public string MappingsFolder {
            get {
                return this["mappingsFolder"].ToString();
            }
            set {
                this["mappingsFolder"] = value;
            }
        }

        /// <summary>
        /// Identifica se as strings de conexão estão encriptadas.
        /// </summary>
        [ConfigurationProperty("useEncryptedConnectionString", DefaultValue = "false", IsRequired = false)]
        public bool UseEncryptedConnectionString {
            get {
                return (bool)this["useEncryptedConnectionString"];
            }
            set {
                this["useEncryptedConnectionString"] = value;
            }
        }

        /// <summary>
        /// Strings de conexão da aplicação.
        /// </summary>
        [ConfigurationProperty("databaseConnections", IsRequired = true)]
        public DatabaseConnectionCollection Connections {
            get { return this["databaseConnections"] as DatabaseConnectionCollection; }
            set { this["databaseConnections"] = value; }
        }

        /// <summary>
        /// Dados das conexões para acesso ao banco de dados.
        /// </summary>
        public List<ConnectionInfo> DatabaseConnections {
            get {
                var _databaseConnections = new List<ConnectionInfo>();

                #region Obtenção das strings de conexão

                for (int i = 0; i < this.Connections.Count; i++) {
                    var conn = this.Connections[i];

                    //Verifica se o provider é um dos drivers disponíveis.
                    if (conn.ProviderName.IndexOf("Intentor.Yamapper.Drivers") >= 0) {
                        ConnectionInfo info = new ConnectionInfo();

                        info.Name = conn.Name;
                        info.ConnectionString = (this.UseEncryptedConnectionString ? CryptoHelper.Decrypt(conn.ConnectionString) : conn.ConnectionString);
                        info.ProviderName = conn.ProviderName;

                        try {
                            if (!conn.Parameters.IsNullOrEmpty()) {
                                string[] parameters = conn.Parameters.Split(';');

                                //Adiciona os parâmetros ao dicionário.
                                foreach (var param in parameters) {
                                    string[] paramData = param.Split(':');
                                    info.Parameters.Add(paramData[0], paramData[1]);
                                }
                            }
                        } catch { }

                        _databaseConnections.Add(info);
                    }
                }

                #endregion

                return _databaseConnections;
            }
        }

        /// <summary>
        /// Obtém dados de uma conexão a partir de seu nome.
        /// </summary>
        /// <param name="connectionName">Nome da conexão.</param>
        /// <returns>Objeto <see cref="ConnectionInfo"/>.</returns>
        public ConnectionInfo GetConnectionByName(string connectionName) {
            ConnectionInfo c = null;

            foreach (ConnectionInfo conn in this.DatabaseConnections) {
                if (conn.Name.Equals(connectionName)) {
                    c = conn;
                    break;
                }
            }

            return c;
        }

        /// <summary>
        /// Cria uma instância do objeto.
        /// </summary>
        /// <returns>Retorna uma instância do objeto.</returns>
        public static YamapperConfigurationSection GetInstance() {
            return (YamapperConfigurationSection)System.Configuration.ConfigurationManager.GetSection(
                "intentor/yamapper");
        }

        #region Permissions

        /// <summary>
        /// Representa conexões de banco de dados.
        /// </summary>
        public class DatabaseConnectionCollection
            : ConfigurationElementCollection {
            /// <summary>
            /// Conexões com o banco de dados.
            /// </summary>
            /// <param name="index">Índice da conexão a ser obtida.</param>
            /// <returns>Objeto <see cref="ConnectionDataElement"/>.</returns>
            public ConnectionDataElement this[int index] {
                get { return base.BaseGet(index) as ConnectionDataElement; }
                set {
                    if (base.BaseGet(index) != null) {
                        base.BaseRemoveAt(index);
                    }
                    this.BaseAdd(index, value);
                }
            }

            protected override ConfigurationElement CreateNewElement() {
                return new ConnectionDataElement();
            }

            protected override object GetElementKey(ConfigurationElement element) {
                return ((ConnectionDataElement)element).Name;
            }
        }

        /// <summary>
        /// Elemento de dados de uma conexão com o banco de dados.
        /// </summary>
        public class ConnectionDataElement
            : ConfigurationElement {
            /// <summary>
            /// Nome da conexão com o banco de dados.
            /// </summary>
            [ConfigurationProperty("name", IsRequired = true)]
            public string Name {
                get { return this["name"].ToString(); }
                set { this["name"] = value; }
            }

            /// <summary>
            /// String de conexão do banco de dados.
            /// </summary>
            [ConfigurationProperty("connectionString", IsRequired = true)]
            public string ConnectionString {
                get { return this["connectionString"].ToString(); }
                set { this["connectionString"] = value; }
            }

            /// <summary>
            /// Provedor de acesso do banco de dados.
            /// </summary>
            [ConfigurationProperty("provider", IsRequired = true)]
            public string ProviderName {
                get { return this["provider"].ToString(); }
                set { this["provider"] = value; }
            }

            /// <summary>
            /// Parâmetros do provedor de acesso ao banco de dados.
            /// </summary>
            [ConfigurationProperty("params", IsRequired = false)]
            public string Parameters {
                get { return this["params"].ToString(); }
                set { this["params"] = value; }
            }
        }

        #endregion
    }
}
