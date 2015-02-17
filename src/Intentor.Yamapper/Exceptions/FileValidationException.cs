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
    /// Exceção de falha de validação de arquivo.
    /// </summary>
    [Serializable]
    public sealed class FileValidationException :
        System.Exception {
        public FileValidationException()
            :
            this(null) { }

        public FileValidationException(string message)
            :
            this(message, null) { }

        public FileValidationException(string message, Exception innerException)
            :
            base(message ?? Messages.FileValidationException, innerException) { }

        private FileValidationException(SerializationInfo info, StreamingContext context)
            :
            base(info, context) { }
    }
}
