using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEolQuery : IRequest<AlunoPorTurmaResposta>
    {
        public ObterAlunoPorCodigoEolQuery(string codigoAluno, int anoLetivo, bool historico = false , string codigoTurma = null)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            CodigoTurma = codigoTurma;
            Historico = historico;
        }

        public string CodigoAluno { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoTurma { get; set; }
        public bool Historico { get; set; } = false;
    }

    public class ObterAlunoPorCodigoEolQueryValidator : AbstractValidator<ObterAlunoPorCodigoEolQuery>
    {
        public ObterAlunoPorCodigoEolQueryValidator()
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
