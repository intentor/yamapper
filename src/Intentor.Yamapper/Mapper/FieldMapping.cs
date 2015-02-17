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
    /// Identifica dados de mapeamento de uma propriedade
    /// </summary>
    internal class FieldMapping {
        #region Propriedades

        /// <summary>
        /// Nome da propriedade na entidade.
        /// </summary>
        public string PropertyName {
            get;
            set;
        }

        /// <summary>
        /// Nome do campo no banco de dados.
        /// </summary>
        public string FieldName {
            get;
            set;
        }

        /// <summary>
        /// Identifica se o campo permite valores nulos.
        /// </summary>
        public bool AllowNull {
            get;
            set;
        }

        /// <summary>
        /// Identifica se o campo é primary key.
        /// </summary>
        public bool IsPrimaryKey {
            get;
            set;
        }

        /// <summary>
        /// Identifica se a coluna é de autonumeração.
        /// </summary>
        public bool IsAutoKey {
            get;
            set;
        }

        /// <summary>
        /// Nome da sequence para preenchimento do campo.
        /// </summary>
        public string SequenceName {
            get;
            set;
        }

        /// <summary>
        /// Identifica se a coluna deve ser ignorada quando da criação do registro.
        /// </summary>
        public bool IgnoreOnInsert {
            get;
            set;
        }

        /// <summary>
        /// Identifica se a coluna deve ser ignorada quando da atualização do registro.
        /// </summary>
        public bool IgnoreOnUpdate {
            get;
            set;
        }

        #endregion
    }
}
