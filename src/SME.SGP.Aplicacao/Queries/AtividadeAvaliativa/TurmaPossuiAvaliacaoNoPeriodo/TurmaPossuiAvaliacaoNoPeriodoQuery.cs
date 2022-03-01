using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class TurmaPossuiAvaliacaoNoPeriodoQuery : IRequest<bool>
    {
        public TurmaPossuiAvaliacaoNoPeriodoQuery(long turmaId, long periodoEscolarId, long? componenteCurricularCodigo = null)
        {
            TurmaId = turmaId;
            PeriodoEscolarId = periodoEscolarId;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public long TurmaId { get; }
        public long PeriodoEscolarId { get; }
        public long? ComponenteCurricularCodigo { get; }
    }

    public class TurmaPossuiAvaliacaoNoPeriodoQueryValidator : AbstractValidator<TurmaPossuiAvaliacaoNoPeriodoQuery>
    {
        public TurmaPossuiAvaliacaoNoPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O identificador da turma deve ser informado para consulta de avaliações no bimestre");

            RuleFor(a => a.PeriodoEscolarId)
                .NotEmpty()
                .WithMessage("O periodo escolar deve ser informado para consulta de avaliações no bimestre");
        }
    }
}
