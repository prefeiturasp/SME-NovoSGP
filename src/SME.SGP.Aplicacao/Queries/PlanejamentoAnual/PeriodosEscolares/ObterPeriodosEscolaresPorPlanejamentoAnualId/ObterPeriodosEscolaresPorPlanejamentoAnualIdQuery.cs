using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery : IRequest<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>>
    {
        public ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery(long planejamentoAnualId)
        {
            PlanejamentoAnualId = planejamentoAnualId;
        }

        public long PlanejamentoAnualId { get; set; }
    }

    public class ObterPeriodosEscolaresPorPlanejamentoAnualIdQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery>
    {
        public ObterPeriodosEscolaresPorPlanejamentoAnualIdQueryValidator()
        {
            RuleFor(c => c.PlanejamentoAnualId)
                .NotEmpty()
                .WithMessage("O id do planejamento anual deve ser informado.");
        }
    }
}
