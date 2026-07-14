using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery : IRequest<IEnumerable<long>>
    {
        public string CodigoAluno { get; set; }
        public long TurmaId { get; set; }

        public ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery(string codigoAluno, long turmaId)
        {
            CodigoAluno = codigoAluno;
            TurmaId = turmaId;
        }
    }

    public class ObterConsolidacoesAtivasIdPorAlunoETurmaQueryValidator : AbstractValidator<ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery>
    {
        public ObterConsolidacoesAtivasIdPorAlunoETurmaQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consultar as consolidações de conselho ativas dele nessa turma");

            RuleFor(a => a.TurmaId)
              .NotEmpty()
              .WithMessage("É necessário informar o id da turma do aluno para consultar as consolidações de conselho ativas dele nessa turma");
        }
    }
}
