/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Intentor.Yamapper {
    /// <summary>
    /// Exceção de chave primária não encontrada.
    /// </summary>
    [Serializable]
    public sealed class PrimaryKeyNotFoundException :
        System.Exception {
        public PrimaryKeyNotFoundException()
            :
            this(null) { }

        public PrimaryKeyNotFoundException(string message)
            :
            this(message, null) { }

        public PrimaryKeyNotFoundException(string message, Exception innerException)
            :
            base(message ?? Messages.PrimaryKeyNotFoundException, innerException) { }

        private PrimaryKeyNotFoundException(SerializationInfo info, StreamingContext context)
            :
            base(info, context) { }
    }
}
