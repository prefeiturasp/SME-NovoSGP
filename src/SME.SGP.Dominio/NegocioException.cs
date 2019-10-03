using System;

namespace SME.SGP.Dominio
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem, int statusCode = 601) : base(mensagem)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}