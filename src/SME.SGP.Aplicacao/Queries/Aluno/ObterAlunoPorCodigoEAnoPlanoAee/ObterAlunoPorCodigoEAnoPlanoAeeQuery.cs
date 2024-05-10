using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEAnoPlanoAeeQuery : IRequest<AlunoReduzidoDto>
    {
        public ObterAlunoPorCodigoEAnoPlanoAeeQuery(string codigoAluno, int anoLetivo, bool tipoTurma = false)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            TipoTurma = tipoTurma;
        }

        public string CodigoAluno { get; set; }
        public int AnoLetivo { get; set; }
        public bool TipoTurma { get; set; }
    }

    public class ObterAlunoPorCodigoEAnoPlanoAeeQueryValidator : AbstractValidator<ObterAlunoPorCodigoEAnoPlanoAeeQuery>
    {
        public ObterAlunoPorCodigoEAnoPlanoAeeQueryValidator()
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