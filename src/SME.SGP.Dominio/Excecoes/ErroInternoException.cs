using System;
using System.Runtime.Serialization;

namespace SME.SGP.Dominio
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
