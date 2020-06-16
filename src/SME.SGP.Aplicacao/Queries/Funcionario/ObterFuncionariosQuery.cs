using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.Funcionario
{
    public class ObterFuncionariosQuery : IRequest<FiltroFuncionariosDto>
    {
        public string CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public ObterFuncionariosQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = CodigoUe;
        }
    }

    public class ObterFuncionariosQueryValidator : AbstractValidator<ObterFuncionariosQuery>
    {
        public ObterFuncionariosQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da DRE é obrigatório.");
        }
    }
}
