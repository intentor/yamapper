/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;

namespace Intentor.Yamapper {
    /// <summary>
    /// Identifica métodos básicos para acesso ao banco de dados.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade que representa as tabelas do banco de dados. </typeparam>
    public interface ICommonDataBaseActions<T> : IViewDataBaseActions<T> {
        /// <summary>
        /// Cria um registro na base de dados com base em um objeto.
        /// </summary>
        /// <param name="entity">Objeto a ser criado.</param>
        void Create(T obj);

        /// <summary>
        /// Atualiza um registro na base de dados com base em um objeto.
        /// </summary>
        /// <param name="entity">Objeto a ser atualizado.</param>
        void Update(T obj);

        /// <summary>
        /// Delete um registro da base de dados com base em critérios.
        /// </summary>
        /// <param name="crit">Critérios de exclusão do registro.</param>
        /// <returns>Quantidade de registros excluídos.</returns>
        int Delete(Criteria crit);
    }
}