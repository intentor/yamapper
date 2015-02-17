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
    /// Define uma cláusula de ordenação.
    /// </summary>
    public sealed class Order {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.
        /// </param>
        /// <param name="ascending">Identifica se se deve ordernar de forma ascendente ou descendente.
        /// </param>
        public Order(string propertyName
            , bool ascending) {
            this.PropertyName = propertyName;
            this.Ascending = ascending;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Nome da propriedade da entidade.
        /// </summary>
        public string PropertyName {
            get;
            set;
        }

        /// <summary>
        /// Identifica se se deve ordernar de forma ascendente ou descendente.
        /// </summary>
        public bool Ascending {
            get;
            set;
        }

        #endregion

        #region Métodos static

        /// <summary>
        /// Cria uma nova ordenação ascendente.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <returns>Objeto <see cref="Order"/>.</returns>
        public static Order ASC(string propertyName) {
            return new Order(propertyName, true);
        }

        /// <summary>
        /// Cria uma nova ordenação descendente.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade da entidade.</param>
        /// <returns>Objeto <see cref="Order"/>.</returns>
        public static Order DESC(string propertyName) {
            return new Order(propertyName, false);
        }

        #endregion
    }
}
