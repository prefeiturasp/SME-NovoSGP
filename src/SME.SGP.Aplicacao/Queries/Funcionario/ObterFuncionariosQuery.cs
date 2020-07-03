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

        public string CodigoRf { get; set; }

        public string NomeServidor { get; set; }

        public ObterFuncionariosQuery() { }

        public ObterFuncionariosQuery(string codigoDre, 
                                      string codigoUe,
                                      string codigoRf,
                                      string nomeServidor)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            CodigoRf = codigoRf;
            NomeServidor = nomeServidor;
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
