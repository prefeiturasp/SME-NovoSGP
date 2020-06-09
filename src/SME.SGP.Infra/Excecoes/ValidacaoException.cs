using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Excecoes
{
    public class ValidacaoException : Exception
    {
        public readonly IEnumerable<ValidationFailure> Erros;

        public ValidacaoException(IEnumerable<ValidationFailure> erros)
        {
            this.Erros = erros;
        }
    }
}
