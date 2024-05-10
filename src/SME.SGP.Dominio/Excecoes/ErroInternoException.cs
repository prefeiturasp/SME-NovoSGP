using System;
using System.Runtime.Serialization;

namespace SME.SGP.Dominio
{
    [Serializable]
    public class ErroInternoException : Exception
    {
        public ErroInternoException(string mensagem)
        : base(mensagem) { }

        public ErroInternoException(string mensagem, Exception innerException) 
            : base(mensagem, innerException) { }

        protected ErroInternoException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
