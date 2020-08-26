using System;
using System.Runtime.Serialization;

namespace SME.Background.Core.Exceptions
{
    [Serializable]
    public class ExcecaoServicoDesativadoException : Exception
    {
        public ExcecaoServicoDesativadoException(string mensagem)
          : base(mensagem) { }

        protected ExcecaoServicoDesativadoException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
