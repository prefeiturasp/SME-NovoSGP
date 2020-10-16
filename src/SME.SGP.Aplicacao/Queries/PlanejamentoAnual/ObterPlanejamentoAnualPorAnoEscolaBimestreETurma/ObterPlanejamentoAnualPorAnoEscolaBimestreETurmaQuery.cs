using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery : IRequest<PlanejamentoAnual>
    {
        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery(long turmaId, long periodoEscolarId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryValidator : AbstractValidator<ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQuery>
    {
        public ObterPlanejamentoAnualPorAnoEscolaBimestreETurmaQueryValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para consulta do plano anual.");

            RuleFor(c => c.PeriodoEscolarId)
                .NotEmpty()
                .WithMessage("O periodo escolar deve ser informado para consulta do plano anual.");

            RuleFor(c => c.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado para consulta do plano anual.");
        }
    }
}
