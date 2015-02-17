/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Linq;
using System.Text;

namespace Intentor.Yamapper {
    /// <summary>
    ///     Identifica uma coluna de uma tabela do 
    ///     banco de dados para mapeamento.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ColumnAttribute :
        DbMemberAttribute {
        #region Construtor

        /// <summary>
        ///     Construtor da classe.
        /// </summary>
        public ColumnAttribute() {
        }

        /// <summary>
        ///     Construtor da classe.
        /// </summary>
        /// <param name="name">
        ///     Nome da coluna na base de dados.
        /// </param>
        public ColumnAttribute(string name) {
            this.Name = name;
        }

        /// <summary>
        ///     Construtor da classe.
        /// </summary>
        /// <param name="name">
        ///     Nome da coluna na base de dados.
        /// </param>
        /// <param name="allowNull">
        ///     Identifica se a coluna permite valores nulos.
        /// </param>
        public ColumnAttribute(string name
            , bool allowNull) {
            this.Name = name;
            this.AllowNull = allowNull;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Identifica se a coluna permite valores nulos.
        /// </summary>
        public bool AllowNull {
            get;
            set;
        }

        /// <summary>
        /// Identifica se a coluna é primary key.
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
