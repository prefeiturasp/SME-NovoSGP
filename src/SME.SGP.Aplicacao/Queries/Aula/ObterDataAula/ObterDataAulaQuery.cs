using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDataAulaQuery: IRequest<DateTime>
    {
        public ObterDataAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterDataAulaQueryValidator: AbstractValidator<ObterDataAulaQuery>
    {
        public ObterDataAulaQueryValidator()
        {
            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O Id da Aula deve ser informado.");

        }
    }
}
