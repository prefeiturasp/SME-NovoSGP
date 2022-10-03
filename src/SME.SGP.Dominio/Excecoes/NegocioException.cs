using System;
using System.Net;

namespace SME.SGP.Dominio
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem, int statusCode = 601) : base(mensagem)
        {
            StatusCode = statusCode;
        }

        public NegocioException(string mensagem, HttpStatusCode statusCode) : base(mensagem)
        {
            StatusCode = (int)statusCode;
        }

        public int StatusCode { get; }
    }
}