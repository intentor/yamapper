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
    /// Expressão "AND".
    /// </summary>
    public class AndExpression : LogicalExpression {
        /// <summary>
        /// Cria uma nova expressão lógica "AND".
        /// </summary>
        /// <param name="expressions">Expressões da expressão lógica.</param>
        public AndExpression(params IExpression[] expressions)
            : base(expressions) { }

        protected override string Operator {
            get { return " AND "; }
        }
    }
}
