using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorTurmaComponenteQuery : IRequest<long>
    {
        public ObterPlanejamentoAnualPorTurmaComponenteQuery(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ObterPlanejamentoAnualPorTurmaComponenteQueryValidator : AbstractValidator<ObterPlanejamentoAnualPorTurmaComponenteQuery>
    {
        public ObterPlanejamentoAnualPorTurmaComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada.");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");
        }
    }
}
