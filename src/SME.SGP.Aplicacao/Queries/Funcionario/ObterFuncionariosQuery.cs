using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Funcionario
{
    public class ObterFuncionariosQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public string CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public ObterFuncionariosQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
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
