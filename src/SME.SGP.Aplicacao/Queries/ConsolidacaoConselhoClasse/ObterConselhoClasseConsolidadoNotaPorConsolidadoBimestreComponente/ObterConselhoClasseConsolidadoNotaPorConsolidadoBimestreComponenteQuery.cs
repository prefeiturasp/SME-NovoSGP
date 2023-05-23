using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery : IRequest<ConselhoClasseConsolidadoTurmaAlunoNota>
    {
        public ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery(long consolidadoTurmaAlunoId, int? bimestre, long componenteCurricularId)
        {
            ConsolidadoTurmaAlunoId = consolidadoTurmaAlunoId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ConsolidadoTurmaAlunoId { get; }
        public int? Bimestre { get; }
        public long ComponenteCurricularId { get; }
    }

    public class ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQueryValidator : AbstractValidator<ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQuery>
    {
        public ObterConselhoClasseConsolidadoNotaPorConsolidadoBimestreComponenteQueryValidator()
        {
            RuleFor(a => a.ConsolidadoTurmaAlunoId)
              .NotEmpty()
              .WithMessage("É necessário informar o id da consolidação turma aluno para consultar as consolidações de conselho por turma, aluno e nota");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para consultar as consolidações de conselho por turma, aluno e nota");
        }
    }
}
