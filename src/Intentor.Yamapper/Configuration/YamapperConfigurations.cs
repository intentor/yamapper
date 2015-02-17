/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Configurações do componente definidos nos arquivos .config.
    /// </summary>
    public static class YamapperConfigurations {
        #region Construtor

        static YamapperConfigurations() {
            IYamapperConfigurationData configurationData;
            var configurationSection = YamapperConfigurationSection.GetInstance();

            //Verifica se há arquivo de configuração externo.
            if (String.IsNullOrEmpty(configurationSection.PathConfigurationFile)) {
                //Não havendo, carrega os dados a partir de Configuration Section.
                configurationData = configurationSection;
            } else {
                //Havendo arquivo, carrega os dados a partir deste.
                string path = configurationSection.PathConfigurationFile;
                if (!Path.IsPathRooted(path)) path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                configurationData = new YamapperConfigurationFile(path);
            }

            _configurationData = configurationData;
        }

        #endregion

        #region Campos

        private static IYamapperConfigurationData _configurationData;

        #endregion

        #region Propriedades

        /// <summary>
        ///     Identifica o provedor padrão para acesso à base de dados.
        /// </summary>
        public static string DefaultConnection {
            get { return _configurationData.DefaultConnection; }
        }

        /// <summary>
        ///     Identifica o assembly que contém as entidades a serem mapeadas.
        /// </summary>
        public static string MappingAssembly {
            get { return _configurationData.MappingAssembly; }
        }

        // <summary>
        ///     Identifica o diretório que contém os arquivos XML de mapeamento 
        ///     das tabelas.
        /// </summary>
        public static string MappingsFolder {
            get { return _configurationData.MappingsFolder; }
        }

        /// <summary>
        ///     Dados das conexões para acesso ao banco de dados.
        /// </summary>
        public static List<ConnectionInfo> DatabaseConnections {
            get { return _configurationData.DatabaseConnections; }
        }

        #endregion
    }
}
