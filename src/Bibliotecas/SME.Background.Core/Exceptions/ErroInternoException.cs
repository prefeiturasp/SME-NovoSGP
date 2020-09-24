using System;
using System.Runtime.Serialization;

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
