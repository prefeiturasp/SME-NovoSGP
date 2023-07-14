using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoConselhoPorAlunosQuery : IRequest<IEnumerable<TurmaAlunoDto>>
    {
        public ObterTurmasFechamentoConselhoPorAlunosQuery(long[] alunosCodigos, int anoLetivo)
        {
            AlunosCodigos = alunosCodigos;
            AnoLetivo = anoLetivo;
        }

        public long[] AlunosCodigos { get; set; }
        public int AnoLetivo { get; set; }


    }

    public class ObterTurmasFechamentoConselhoPorAlunosQueryValidator : AbstractValidator<ObterTurmasFechamentoConselhoPorAlunosQuery>
    {
        public ObterTurmasFechamentoConselhoPorAlunosQueryValidator()
        {
            RuleFor(c => c.AlunosCodigos)
                .NotEmpty()
                .WithMessage("O(s) código(s) de aluno(s) deve(m) ser informado(s).");
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
