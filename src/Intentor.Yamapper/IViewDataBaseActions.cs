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
    /// Identifica métodos básicos de obtenção de dados.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade que representa as tabelas do banco de dados. </typeparam>
    public interface IViewDataBaseActions<T> {
        /// <summary>
        /// Obtém todos os registros de uma determinada tabela.
        /// </summary>
        /// <returns>Retorna uma lista contendo todos os objetos do tipo <typeparamref name="T"/> presentes na base de dados.</returns>
        List<T> GetAll();

        /// <summary>
        /// Obtém registros com base em critérios.
        /// </summary>
        /// <param name="crit">Critérios de pesquisa.</param>
        /// <returns>Lista de objetos do tipo <typeparamref name="T"/>.</returns>
        List<T> GetByCriteria(Criteria crit);

        /// <summary>
        /// Conta a quantidade de registros da tabela.
        /// </summary>
        /// <returns>Quantidade de registros da tabela.</returns>
        int Count();

        /// <summary>
        /// Conta a quantidade de registros da tabela a partir de critérios.
        /// </summary>
        /// <param name="crit">Objeto <see cref="Criteria"/> contendo os critérios para obtenção dos dados.</param>
        /// <returns>Quantidade de registros da tabela conforme os critérios informados.</returns>
        int Count(Criteria crit);

        /// <summary>
        /// Verifica se há registros para uma determinada entidade a partir critérios.
        /// </summary>
        /// <param name="crit">Objeto <see cref="Criteria"/> contendo os critérios para obtenção dos dados.</param>
        /// <returns>Valor booleano indicando se há registros.</returns>
        bool Exists(Criteria crit);

        /// <summary>
        /// Verifica se uma tabela possui dados.
        /// </summary>
        /// <returns>Valor booleano indicando se a tabela possui dados.</returns>
        bool HasRows();
    }
}