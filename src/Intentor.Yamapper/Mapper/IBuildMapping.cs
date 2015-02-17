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
    /// Interface para realização de criação de mapeamento de objetos.
    /// </summary>
    internal interface IBuildMapping {
        /// <summary>
        /// Realiza criação dos objetos de mapeamento  para acesso ao banco de dados.
        /// </summary>
        /// <returns>Objeto <see cref="TableMappingCollection"/> representando os objetos de mapeamento.</returns>
        TableMappingCollection Build();
    }
}
