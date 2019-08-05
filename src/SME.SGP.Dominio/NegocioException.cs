using System;

namespace SME.SGP.Dominio
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem):base(mensagem)
        {
        }
    }
}
