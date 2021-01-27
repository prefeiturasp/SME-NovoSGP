using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery : IRequest<bool>
    {
        public VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(long turmaId, long componenteCurricularId, long? periodoEscolarId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long? PeriodoEscolarId { get; set; }
    }

    public class VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQueryValidator : AbstractValidator<VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery>
    {
        public VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da turma");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id do componente curricular");
        }
    }
}
