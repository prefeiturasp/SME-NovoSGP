using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPeriodoEscolarPorIdQuery : IRequest<PlanejamentoAnualPeriodoEscolar>
    {
        public ObterPlanejamentoAnualPeriodoEscolarPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterPlanejamentoAnualPeriodoEscolarPorIdQueryValidator : AbstractValidator<ObterPlanejamentoAnualPeriodoEscolarPorIdQuery>
    {
        public ObterPlanejamentoAnualPeriodoEscolarPorIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("O id do planejamento anual periodo escolar ser informado.");
        }
    }
}
