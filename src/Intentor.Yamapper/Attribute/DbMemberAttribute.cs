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
    /// Atributo abstrato para criação de outros tipos de atributos relacionados ao mapeamento de tabelas e colunas.
    /// </summary>
    public abstract class DbMemberAttribute :
        System.Attribute {
        #region Construtor

        protected DbMemberAttribute() {
        }

        #endregion

        #region Campos

        /// <summary>
        /// Nome do membro base de dados.
        /// </summary>
        protected string _name;

        /// <summary>
        /// Identifica se se deve utilizar o nome da propriedade como nome do membro na base de dados.
        /// </summary>
        protected bool _usePropertyNameAsMemberName;

        #endregion

        #region Propriedades

        /// <summary>
        /// Nome do membro na base de dados.
        /// </summary>
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Identifica se se deve utilizar o nome da propriedade como nome do membro na base de dados.
        /// </summary>
        public bool UsePropertyNameAsMemberName {
            get {
                _usePropertyNameAsMemberName = String.IsNullOrEmpty(_name);

                return _usePropertyNameAsMemberName;
            }
            set { _usePropertyNameAsMemberName = value; }
        }

        #endregion
    }
}
