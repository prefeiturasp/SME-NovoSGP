using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEmManutencaoQuery: IRequest<bool>
    {
        public ObterAulaEmManutencaoQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterAulaEmManutencaoQueryValidator: AbstractValidator<ObterAulaEmManutencaoQuery>
    {
        public ObterAulaEmManutencaoQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da aula para consultar registro em manutenção.");
        }
    }
}
