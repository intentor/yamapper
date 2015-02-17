/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Text;

namespace Intentor.Yamapper {
    /// <summary>
    /// Identifica uma tabela do banco de dados para mapeamento.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TableAttribute :
        DbMemberAttribute {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        public TableAttribute() {
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="name">Nome da tabela na base de dados.</param>
        /// <param name="connectionName">Nome da conexão para acesso ao banco de dados.</param>
        public TableAttribute(string name,
            string connectionName) {
            this.Name = name;
            this.ConnectionName = connectionName;
        }

        #endregion

        #region Propridades

        /// <summary>
        /// Identifica se o mapeamento representa uma view.
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// Indica se se deve realizar carregamento por demanda dos dados da entidade.
        /// </summary>
        public bool IsLazyLoading { get; set; }

        /// <summary>
        /// Nome da conexão para acesso ao banco de dados.
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// Nome da sequence para obtenção do valor da primary key.
        /// </summary>
        public string SequenceName { get; set; }

        #endregion
    }
}
