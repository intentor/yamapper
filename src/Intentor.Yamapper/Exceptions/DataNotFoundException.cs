/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Runtime.Serialization;

namespace Intentor.Yamapper {
    /// <summary>
    /// Exceção de dados não encontrados.
    /// </summary>
    [Serializable]
    public sealed class DataNotFoundException :
        System.Exception {
        public DataNotFoundException()
            :
            this(null) { }

        public DataNotFoundException(string message)
            :
            this(message, null) { }

        public DataNotFoundException(string message, Exception innerException)
            :
            base(message ?? Messages.DataNotFoundException, innerException) { }

        private DataNotFoundException(SerializationInfo info, StreamingContext context)
            :
            base(info, context) { }
    }
}
