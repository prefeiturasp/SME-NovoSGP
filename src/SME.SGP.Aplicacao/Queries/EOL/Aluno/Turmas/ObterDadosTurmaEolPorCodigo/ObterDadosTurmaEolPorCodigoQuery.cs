using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosTurmaEolPorCodigoQuery : IRequest<DadosTurmaEolDto>
    {
        public ObterDadosTurmaEolPorCodigoQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }
    }

    public class ObterDadosTurmaEolPorCodigoQueryValidator : AbstractValidator<ObterDadosTurmaEolPorCodigoQuery>
    {
        public ObterDadosTurmaEolPorCodigoQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("Informe o código da Turma para Obter Dados da Turma");
        }
    }
}