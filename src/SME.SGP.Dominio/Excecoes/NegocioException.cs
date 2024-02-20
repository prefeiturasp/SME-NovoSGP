using System;
using System.Net;

namespace SME.SGP.Dominio
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem, int statusCode = 601, bool ehInformativo = false) : base(mensagem)
        {
            StatusCode = statusCode;
            EhInformatico = ehInformativo;
        }

        public NegocioException(string mensagem, HttpStatusCode statusCode) : base(mensagem)
        {
            StatusCode = (int)statusCode;
        }

        public NegocioException(string mensagem, Exception innerException) : base(mensagem, innerException)
        { }

        public int StatusCode { get; }
        public bool EhInformatico { get; }
    }
}