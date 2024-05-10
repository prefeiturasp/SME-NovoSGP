using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorIdsQuery : IRequest<IEnumerable<Dominio.Aula>>
    {
        public ObterAulasPorIdsQuery(IEnumerable<long> aulasIds)
        {
            AulasIds = aulasIds;
        }

        public IEnumerable<long> AulasIds { get; }
    }

    public class ObterAulasPorIdsQueryValidator : AbstractValidator<ObterAulasPorIdsQuery>
    {
        public ObterAulasPorIdsQueryValidator()
        {
            RuleFor(x => x.AulasIds)
                .NotEmpty()
                .WithMessage("Os identificadores de aulas precisam ser informados para sua consulta");
        }
    }
}
