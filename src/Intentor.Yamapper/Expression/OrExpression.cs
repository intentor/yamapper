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
    /// Expressão "OR".
    /// </summary>
    public class OrExpression : LogicalExpression {
        /// <summary>
        /// Cria uma nova expressão lógica "OR".
        /// </summary>
        /// <param name="expressions">Expressões da expressão lógica.</param>
        public OrExpression(params IExpression[] expressions)
            : base(expressions) { }

        protected override string Operator {
            get { return " OR "; }
        }
    }
}
