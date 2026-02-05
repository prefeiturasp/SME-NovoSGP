using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterPlanoAEE
{
    public class ObterConsolidacaoPlanosAEEQuery : IRequest<IEnumerable<PainelEducacionalPlanoAEEDto>>
    {
        public ObterConsolidacaoPlanosAEEQuery(FiltroPainelEducacionalPlanosAEE filtro)
        {
            Filtro = filtro;
        }
        public FiltroPainelEducacionalPlanosAEE Filtro { get; set; }
    }

    public class ObterPlanosAEEConsolidadosQueryValidator : AbstractValidator<ObterConsolidacaoPlanosAEEQuery>
    {
        public ObterPlanosAEEConsolidadosQueryValidator()
        {
            RuleFor(c => c.Filtro.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
