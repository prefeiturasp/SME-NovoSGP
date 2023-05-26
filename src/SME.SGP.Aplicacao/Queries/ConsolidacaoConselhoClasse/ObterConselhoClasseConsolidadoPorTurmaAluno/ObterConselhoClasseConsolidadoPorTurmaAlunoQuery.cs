using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseConsolidadoPorTurmaAlunoQuery : IRequest<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ObterConselhoClasseConsolidadoPorTurmaAlunoQuery(long turmaId, string alunoCodigo)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public string AlunoCodigo { get; }
    }

    public class ObterConselhoClasseConsolidadoPorTurmaAlunoQueryValidator : AbstractValidator<ObterConselhoClasseConsolidadoPorTurmaAlunoQuery>
    {
        public ObterConselhoClasseConsolidadoPorTurmaAlunoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
              .NotEmpty()
              .WithMessage("É necessário informar o id da turma do aluno para consultar as consolidações de conselho por turma e aluno");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consultar as consolidações de conselho por turma e aluno");
        }
    }
}
