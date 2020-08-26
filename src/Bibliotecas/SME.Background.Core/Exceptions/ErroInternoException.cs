using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SME.Background.Core.Exceptions
{
    [Serializable]
    public class ErroInternoException : Exception
    {
        public ErroInternoException(string mensagem)
        : base(mensagem) { }

        protected ErroInternoException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
