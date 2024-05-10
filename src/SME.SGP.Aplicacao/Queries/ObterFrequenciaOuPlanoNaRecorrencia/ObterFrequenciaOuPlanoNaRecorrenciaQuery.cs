using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaOuPlanoNaRecorrenciaQuery: IRequest<bool>
    {
        public ObterFrequenciaOuPlanoNaRecorrenciaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterFrequenciaOuPlanoNaRecorrenciaQueryValidator: AbstractValidator<ObterFrequenciaOuPlanoNaRecorrenciaQuery>
    {
        public ObterFrequenciaOuPlanoNaRecorrenciaQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado para consulta da existência de frequência ou plano");
        }
    }
}
