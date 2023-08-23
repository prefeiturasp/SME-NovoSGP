using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordosPorAulaQuery : IRequest<IEnumerable<DiarioBordo>>
    {
        public ObterDiariosDeBordosPorAulaQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterDiariosDeBordosPorAulaQueryValidator : AbstractValidator<ObterDiariosDeBordosPorAulaQuery>
    {
        public ObterDiariosDeBordosPorAulaQueryValidator()
        {
            RuleFor(c => c.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado.");
        }
    }
}
