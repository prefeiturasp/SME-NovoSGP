using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery : IRequest<IEnumerable<PlanejamentoAnualPeriodoEscolar>>
    {
        public ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }

    public class ObterPlanejamentoAnualPeriodoEscolarCompletoPorIdQueryValidator : AbstractValidator<ObterPlanejamentoAnualPeriodosEscolaresCompletoPorIdQuery>
    {
        public ObterPlanejamentoAnualPeriodoEscolarCompletoPorIdQueryValidator()
        {
            RuleFor(c => c.Ids)
                .NotEmpty()
                .WithMessage("O id do planejamento anual periodo escolar ser informado.");
        }
    }
}
