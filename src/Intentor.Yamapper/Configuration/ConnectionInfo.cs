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
    ///     Identifica dados de uma conexão com o banco de dados.
    /// </summary>
    public sealed class ConnectionInfo {
        /// <summary>
        ///     Cria um novo objeto.
        /// </summary>
        public ConnectionInfo() {
            this.Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Nome da conexão.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     String de conexão.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Nome do provedor de acesso.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        ///     Parâmetros enviados à strings de conexão.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}
