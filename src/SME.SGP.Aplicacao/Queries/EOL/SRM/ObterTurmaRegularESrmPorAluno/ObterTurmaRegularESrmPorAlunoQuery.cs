using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaRegularESrmPorAlunoQuery : IRequest<IEnumerable<TurmasDoAlunoDto>>
    {
        public ObterTurmaRegularESrmPorAlunoQuery(long alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }

        public long AlunoCodigo { get; }
    }

    public class ObterTurmaRegularESrmPorAlunoQueryValidator : AbstractValidator<ObterTurmaRegularESrmPorAlunoQuery>
    {
        public ObterTurmaRegularESrmPorAlunoQueryValidator()
        {
            RuleFor(t => t.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do aluno deve ser informado para obter as turmas regular e SRM do aluno");
        }
    }
}
