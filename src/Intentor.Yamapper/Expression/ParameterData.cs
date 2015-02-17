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
    /// Informações sobre um parâmetro de acesso ao banco de dados.
    /// </summary>
    public class ParameterData {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ParameterData(string name
            , object value) {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Nome do parâmetro.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Valor do parâmetro.
        /// </summary>
        public object Value {
            get;
            set;
        }
    }
}
