using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosEolPorCodigosEAnoQuery : IRequest<IEnumerable<TurmasDoAlunoDto>>
    {
        public ObterAlunosEolPorCodigosEAnoQuery(long[] codigosAluno, int anoLetivo)
        {
            CodigosAluno = codigosAluno;
            AnoLetivo = anoLetivo;
        }

        public long[] CodigosAluno { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterAlunosEolPorCodigosEAnoQueryValidator : AbstractValidator<ObterAlunosEolPorCodigosEAnoQuery>
    {
        public ObterAlunosEolPorCodigosEAnoQueryValidator()
        {

            RuleFor(c => c.CodigosAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

        }
    }
}
