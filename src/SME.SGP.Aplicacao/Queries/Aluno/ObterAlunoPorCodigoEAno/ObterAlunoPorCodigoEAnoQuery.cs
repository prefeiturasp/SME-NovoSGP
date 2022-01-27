using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEAnoQuery : IRequest<AlunoReduzidoDto>
    {
        public ObterAlunoPorCodigoEAnoQuery(string codigoAluno, int anoLetivo, bool turmaHistorica = false)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            AnoLetivo = anoLetivo;
            TurmaHistorica = turmaHistorica;
        }

        public string CodigoAluno { get; set; }
        public int AnoLetivo { get; set; }
        public bool TurmaHistorica { get; set; }
    }

    public class ObterAlunoPorCodigoEAnoQueryValidator : AbstractValidator<ObterAlunoPorCodigoEAnoQuery>
    {
        public ObterAlunoPorCodigoEAnoQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

        }
    }
}