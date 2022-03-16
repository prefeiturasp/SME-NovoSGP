using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoQuery : IRequest<AlunoReduzidoDto>
    {
        public ObterAlunoPorCodigoQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }
    public class ObterAlunoPorCodigoEAnoHistoricoQueryValidator : AbstractValidator<ObterAlunoPorCodigoQuery>
    {
        public ObterAlunoPorCodigoEAnoHistoricoQueryValidator()
        {

            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");
        }
    }
}
