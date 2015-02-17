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
    /// Operadores lógicos em cláusulas SQL.
    /// </summary>
    public enum LogicalOperator {
        And,
        Or
    }

    /// <summary>
    /// Operadores comuns em cláusulas SQL.
    /// </summary>
    public enum CommonOperator {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterOrEqual,
        LesserThan,
        LesserOrEqual,
        IsNull,
        IsNotNull,
        Like,
        NotLike,
        In,
        NotIn,
        Between
    }
}
