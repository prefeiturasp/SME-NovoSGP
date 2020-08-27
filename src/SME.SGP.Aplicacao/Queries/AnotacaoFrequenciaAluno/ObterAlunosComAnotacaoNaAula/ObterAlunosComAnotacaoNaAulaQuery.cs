using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComAnotacaoNaAulaQuery : IRequest<IEnumerable<string>>
    {
        public ObterAlunosComAnotacaoNaAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterAlunosComAnotacaoNaAulaQueryValidator : AbstractValidator<ObterAlunosComAnotacaoNaAulaQuery>
    {
        public ObterAlunosComAnotacaoNaAulaQueryValidator()
        {
            RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O id da aula deve ser informado.");

        }
    }
}
