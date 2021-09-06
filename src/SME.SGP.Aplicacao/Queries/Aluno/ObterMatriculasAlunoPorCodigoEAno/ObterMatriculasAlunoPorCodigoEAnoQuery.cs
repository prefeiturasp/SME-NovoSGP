using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoPorCodigoEAnoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterMatriculasAlunoPorCodigoEAnoQuery(string codigoAluno, int anoLetivo)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
        }

        public string CodigoAluno { get; }
        public int AnoLetivo { get; }
    }

    public class ObterMatriculasAlunoPorCodigoEAnoQueryValidator : AbstractValidator<ObterMatriculasAlunoPorCodigoEAnoQuery>
    {
        public ObterMatriculasAlunoPorCodigoEAnoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de suas matriculas");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta das matriculas do aluno");
        }
    }
}
