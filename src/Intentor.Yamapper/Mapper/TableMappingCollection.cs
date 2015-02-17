/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections;
using System.Linq;

namespace Intentor.Yamapper {
    internal class TableMappingCollection : CollectionBase {
        #region Indexers

        /// <summary>
        ///     Obtém o mapeamento de uma tabela a partir 
        ///     de seu nome de entidade.
        /// </summary>
        /// <param name="entityName">
        ///     Nome da entidade da tabela.
        /// </param>
        /// <returns>
        ///     Retorna um objeto <see cref="TableMapping"/>
        ///     contendo informações sobre o mapeamento da tabela.
        /// </returns>
        public TableMapping this[string entityName] {
            get {
                TableMapping table = (from t in this.List.Cast<TableMapping>()
                                      where t.EntityName.IndexOf(entityName + ", ") == 0
                                      select t)
                                      .FirstOrDefault();

                return table;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        ///     Adiciona um novo mapeamento de tabela à coleção.
        /// </summary>
        /// <param name="table">
        ///     Objeto <see cref="TableMapping"/> a ser adicionado.
        /// </param>
        /// <remarks>
        ///     A cada inserção é verificado se o mapeamento já existe
        ///     através do nome do objeto. Caso exista, é sobrescrito.
        /// </remarks>
        public void Add(TableMapping table) {
            int? index = null;

            //Verifica se o item já existe.
            for (int i = 0; i < this.List.Count; i++) {
                if (((TableMapping)this.List[i]).EntityName == table.EntityName) {
                    index = i;
                    break;
                }
            }

            if (index.HasValue)
                this.List[index.Value] = table;
            else
                this.List.Add(table);
        }

        #endregion
    }
}
