using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasRfsCriadoresPorNomeQuery : IRequest<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>>
    {
        public ObterItineranciasRfsCriadoresPorNomeQuery(string nomeParaBusca)
        {
            NomeParaBusca = nomeParaBusca;
        }

        public string NomeParaBusca { get; set; }
    }
    public class ObterItineranciasRfsCriadoresPorNomeQueryValidator : AbstractValidator<ObterItineranciasRfsCriadoresPorNomeQuery>
    {
        public ObterItineranciasRfsCriadoresPorNomeQueryValidator()
        {
            RuleFor(c => c.NomeParaBusca)
                .NotEmpty()
                .WithMessage("Informe um nome para a busca")
                .MinimumLength(3)
                .WithMessage("É necessário no mínimo 3 caracteres para efetuar a busca.");

        }
    }
}
