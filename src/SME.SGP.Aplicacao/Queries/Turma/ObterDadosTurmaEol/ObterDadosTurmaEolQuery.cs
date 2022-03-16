using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosTurmaEolQuery : IRequest<DadosTurmaEolDto>
    {
        public ObterDadosTurmaEolQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; private set; }
    }

    public class ObterDadosTurmaEolQueryValidator : AbstractValidator<ObterDadosTurmaEolQuery>
    {
        public ObterDadosTurmaEolQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma.");
        }
    }
}
