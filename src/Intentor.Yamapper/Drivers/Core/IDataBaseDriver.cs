/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Data;
using System.Collections.Generic;
using Intentor.Yamapper.Mapper;

namespace Intentor.Yamapper.Drivers {
    /// <summary>
    /// Interface para definição de estrutura do driver para representação de componentes de um determinado banco de dados a partir de um provider.
    /// </summary>
    interface IDataBaseDriver {
        /// <summary>
        /// Obtém o objeto <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>Objeto <see cref="IDbConnection"/> instanciado.</returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// Obtém o objeto <see cref="IDbDataAdapter"/>.
        /// </summary>
        /// <returns>Objeto <see cref="IDbDataAdapter"/> instanciado.</returns>
        IDbDataAdapter CreateDataAdapter();

        /// <summary>
        /// Formata um nome de parâmetro de acordo com o prefixo definido.
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser formatado.</param>
        /// <returns>Retorna o nome do parâmetro formatado.</returns>
        string FormatNameForParameter(string parameterName);

        /// <summary>
        /// Obtém o comando para limitação de registros em uma consulta.
        /// </summary>
        /// <param name="sql">Comandos SQL a serem executados.</param>
        /// <param name="offset">Posição inicial para obtenção de registros.</param>
        /// <param name="limit">Quantidade máxima de registros a serem retornados.</param>
        /// <returns>Instrução SQL formatada.</returns>
        string GetCommandForLimit(SqlSelectString sql, int offset, int limit);

        /// <summary>
        /// Obtém o comando utilizado para obtenção da primary key de um objeto após sua inserção.
        /// </summary>
        /// <remarks>
        /// Tal valor será colocado diretamente após a instrução de inserção
        /// sem se preocupar em utilizar-se de caracteres de finalização (";"
        /// por exemplo) ou afins para garantir sua execução.
        /// </remarks>
        string GetCommandForPrimaryKeyValue();

        /// <summary>
        /// Parâmetros do driver.
        /// </summary>
        Dictionary<string, string> Parameters { get; }

        /// <summary>
        /// Identifica o caractere utilizado como prefixo para criação de parâmetros nas instruções SQL.
        /// </summary>
        string ParameterPrefix { get; }
    }
}
