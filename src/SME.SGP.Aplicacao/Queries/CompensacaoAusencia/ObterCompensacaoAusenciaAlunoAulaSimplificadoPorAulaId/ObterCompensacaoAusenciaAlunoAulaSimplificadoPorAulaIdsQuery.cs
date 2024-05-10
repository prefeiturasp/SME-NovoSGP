using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto>>
    {
        public ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery(params long[] aulaIds)
        {
            AulaIds = aulaIds;
        }

        public long[] AulaIds { get; }
    }


    public class ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery>
    {
        public ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaIds)
                .NotEmpty()
                .WithMessage("Os ids das aulas devem ser informados.");
        }
    }
}
