/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Intentor.Utilities;
using System.Xml.Serialization;

namespace Intentor.Yamapper {
    /// <summary>
    /// Arquivo de configuração do Yamapper.
    /// </summary>
    //[XmlRoot("yamapper-configuration", Namespace = "urn:yamapper-configuration-10.4.1.0")]
    public class YamapperConfigurationFile
        : IYamapperConfigurationData {
        #region Construtor

        public YamapperConfigurationFile(string pathConfigurationFile) {
            _pathConfigurationFile = pathConfigurationFile;

            var doc = this.LoadFile();
            this.PopulateFields(doc);
        }

        #endregion

        #region Campos

        private const string XmlSchemaNamespace = "urn:yamapper-configuration-10.4.1.0";

        private string _defaultConnection;
        private string _pathConfigurationFile;
        private string _mappingAssembly;
        private string _mappingsFolder;
        private bool _useEncryptedConnectionString;
        private List<ConnectionInfo> _databaseConnections;

        #endregion

        #region Propriedades

        public string DefaultConnection {
            get { return _defaultConnection; }
        }

        public string PathConfigurationFile {
            get { return _pathConfigurationFile; }
        }

        public string MappingAssembly {
            get { return _mappingAssembly; }
        }

        public string MappingsFolder {
            get { return _mappingsFolder; }
        }

        public bool UseEncryptedConnectionString {
            get { return _useEncryptedConnectionString; }
        }

        public List<ConnectionInfo> DatabaseConnections {
            get { return _databaseConnections; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Valida e carrega o arquivo XML de configuração.
        /// </summary>
        /// <returns> Object <see cref="XmlDocument"/>.</returns>
        private XmlDocument LoadFile() {
            XmlSchema configurationSchema =
                XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream("Intentor.Yamapper.Schemas.yamapper-configuration.xsd"), null);

            XmlDocument doc = new XmlDocument();
            doc.Schemas.Add(configurationSchema);
            doc.Load(_pathConfigurationFile);
            doc.Validate((o, e) => {
                if (e.Severity == XmlSeverityType.Error) {
                    throw new FileValidationException(
                        String.Format(Messages.FileValidationExceptionSchema, Path.GetFileName(_pathConfigurationFile), e.Message));
                }
            });

            return doc;
        }

        /// <summary>
        /// Popula os campos da classe.
        /// </summary>
        /// <param name="doc">Arquivo XML de configuração.</param>
        private void PopulateFields(XmlDocument doc) {
            var nameSpaceManager = new XmlNamespaceManager(doc.NameTable);
            nameSpaceManager.AddNamespace("ymap", XmlSchemaNamespace);

            XmlNode baseConfigNode = doc.SelectSingleNode("//ymap:yamapper", nameSpaceManager);

            if (baseConfigNode == null) {
                throw new FileValidationException(
                       String.Format(Messages.FileValidationExceptionNamespace, Path.GetFileName(_pathConfigurationFile), XmlSchemaNamespace));
            }

            XmlNodeList databaseConnectionsNode = doc.SelectNodes("//ymap:connection", nameSpaceManager);

            _defaultConnection = baseConfigNode.Attributes["defaultConnection"].Value;
            _mappingAssembly = baseConfigNode.Attributes["mappingAssembly"].Value;

            _mappingsFolder = baseConfigNode.Attributes["mappingsFolder"].Value;
            if (!System.IO.Path.IsPathRooted(_mappingsFolder)) _mappingsFolder = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, _mappingsFolder);

            _useEncryptedConnectionString = baseConfigNode.Attributes["useEncryptedConnectionString"].Value.Parse<bool>();

            _databaseConnections = new List<ConnectionInfo>(databaseConnectionsNode.Count);
            foreach (XmlNode conn in databaseConnectionsNode) {
                ConnectionInfo info = new ConnectionInfo();

                info.Name = conn.Attributes["name"].Value;
                info.ConnectionString = (this.UseEncryptedConnectionString ? CryptoHelper.Decrypt(conn.Attributes["connectionString"].Value) : conn.Attributes["connectionString"].Value);
                info.ProviderName = conn.Attributes["provider"].Value;

                try {
                    if (!conn.Attributes["params"].Value.IsNullOrEmpty()) {
                        string[] parameters = conn.Attributes["params"].Value.Split(';');

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
    }
}
