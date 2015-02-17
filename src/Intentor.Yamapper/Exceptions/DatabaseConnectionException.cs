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
    /// Exceção relacionada a conexões com o banco de dados.
    /// </summary>
    [Serializable]
    public sealed class DatabaseConnectionException :
        System.Exception {
        public DatabaseConnectionException()
            :
            this(null) { }

        public DatabaseConnectionException(string message)
            :
            this(message, null) { }

        public DatabaseConnectionException(string message, Exception innerException)
            :
            base(message ?? Messages.DatabaseConnectionException, innerException) { }

        private DatabaseConnectionException(SerializationInfo info, StreamingContext context)
            :
            base(info, context) { }
    }
}
