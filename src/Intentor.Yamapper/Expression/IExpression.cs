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
    /// Representa a base de uma expressão de acesso ao banco de dados.
    /// </summary>
    public interface IExpression {
        /// <summary>
        /// Obtém a expressão em formato SQL.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto para obtenção da string.</typeparam>
        /// <param name="parameterPrefix">Prefixo a ser utilizado nos parâmetros.</param>
        /// <param name="parameters">Lista de parâmetros de acesso ao banco de dados.</param>
        /// <returns>String SQL da expressão.</returns>
        string ToSqlString<T>(string parameterPrefix, List<ParameterData> parameters);
    }
}
