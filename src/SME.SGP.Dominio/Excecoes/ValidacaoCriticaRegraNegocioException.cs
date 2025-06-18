using System;

namespace SME.SGP.Dominio
{
    public class ValidacaoCriticaRegraNegocioException : Exception
    {
        public ValidacaoCriticaRegraNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}