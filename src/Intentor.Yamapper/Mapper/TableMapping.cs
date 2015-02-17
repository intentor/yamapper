/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Representa um mapeamento de tabela.
    /// </summary>
    internal class TableMapping {
        #region Campos

        protected List<FieldMapping> _fields = new List<FieldMapping>();

        #endregion

        #region Propriedades

        /// <summary>
        /// Nome da entidade que representa a tabela.
        /// </summary>
        public string EntityName {
            get;
            set;
        }

        /// <summary>
        /// Nome da tabela representada pela entidade.
        /// </summary>
        public string TableName {
            get;
            set;
        }

        /// <summary>
        /// Identifica se o mapeamento representa uma view.
        /// </summary>
        public bool IsView {
            get;
            set;
        }

        /// <summary>
        /// Indica se se deve realizar carregamento por demanda dos dados da entidade.
        /// </summary>
        public bool IsLazyLoading {
            get;
            set;
        }

        /// <summary>
        /// Nome da conexão para acesso ao banco de dados.
        /// </summary>
        public string ConnectionName {
            get;
            set;
        }

        /// <summary>
        /// Mapeamento dos campos da tabela.
        /// </summary>
        public List<FieldMapping> Fields {
            get { return _fields; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Obtém os dados de um campo a partir do nome da propriedade que o representa.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade na entidade que representa a tabela.</param>
        /// <returns>Retorna um objeto <see cref="FieldMapping"/> que representa o campo com o nome informado.</returns>
        public FieldMapping this[string propertyName] {
            get {
                var selectedField = _fields.First(field => field.PropertyName.Equals(propertyName));
                return selectedField;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Indica se a tabela usa sequence, o que indica que não há campos de autonumeração.
        /// </summary>
        /// <returns>Valor booleano indicando se há sequence.</returns>
        public bool HasSequence() {
            return (_fields.Count(f => !f.SequenceName.IsNullOrEmpty()) > 0);
        }

        /// <summary>
        /// Indica se a tabela possui campo de autonumeração.
        /// </summary>
        /// <returns>Valor booleano indicando se há campo de autonumeração.</returns>
        public bool HasAutoKey() {
            return (this.GetAutoKey() != null);
        }

        /// <summary>
        /// Obtém campos que são chave primária.
        /// </summary>
        /// <returns>Retorna lista contendo primary keys da tabela.</returns>
        public List<FieldMapping> GetPrimaryKeys() {
            List<FieldMapping> pks;

            try {
                pks = (from t in _fields
                       where t.IsPrimaryKey.Equals(true)
                       select t).ToList<FieldMapping>();
            } catch {
                pks = null;
            }

            return pks;
        }

        /// <summary>
        /// Obtém campo de autonumeração.
        /// </summary>
        /// <returns>Retorna campo de autonumeração da tabela.</returns>
        public FieldMapping GetAutoKey() {
            return _fields.FirstOrDefault(f => f.IsAutoKey.Equals(true));
        }

        #endregion
    }
}

