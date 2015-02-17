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

namespace Intentor.Yamapper {
    /// <summary>
    /// Representa dados de configuração do componente.
    /// </summary>
    public interface IYamapperConfigurationData {
        /// <summary>
        /// Identifica o provedor padrão para acesso à base de dados.
        /// </summary>
        string DefaultConnection { get; }

        /// <summary>
        /// Identifica a localização do arquivo de configurações do componente.
        /// </summary>
        string PathConfigurationFile { get; }

        /// <summary>
        /// Identifica o assembly que contém as entidades a serem mapeadas.
        /// </summary>
        string MappingAssembly { get; }

        /// <summary>
        /// Identifica o diretório que contém os arquivos XML de mapeamento das tabelas.
        /// </summary>
        string MappingsFolder { get; }

        /// <summary>
        /// Identifica se as strings de conexão estão encriptadas.
        /// </summary>
        bool UseEncryptedConnectionString { get; }

        /// <summary>
        /// Dados das conexões para acesso ao banco de dados.
        /// </summary>
        List<ConnectionInfo> DatabaseConnections { get; }
    }
}
