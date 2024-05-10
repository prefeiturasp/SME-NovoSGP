using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEolPorCodigosQuery : IRequest<IEnumerable<TurmasDoAlunoDto>>
    {
        public ObterAlunosEolPorCodigosQuery(long[] codigosAluno)
        {
            CodigosAluno = codigosAluno;
        }
        public ObterAlunosEolPorCodigosQuery(long codigoAluno, bool todasMatriculas = false)
        {
            CodigosAluno = new long[] { codigoAluno } ;
            TodasMatriculas = todasMatriculas;
        }

        public long[] CodigosAluno { get; set; }
        public bool TodasMatriculas { get; set; }
    }

    public class ObterAlunosEolPorCodigosQueryValidator : AbstractValidator<ObterAlunosEolPorCodigosQuery>
    {
        public ObterAlunosEolPorCodigosQueryValidator()
        {

            RuleFor(c => c.CodigosAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

        }
    }
}
