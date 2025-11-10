using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterEducacaoIntegral
{
    public class ObterEducacaoIntegralQuery : IRequest<IEnumerable<PainelEducacionalEducacaoIntegralDto>>
    {
        public ObterEducacaoIntegralQuery(FiltroPainelEducacionalEducacaoIntegral filtro)
        {
            Filtro = filtro;
        }
        public FiltroPainelEducacionalEducacaoIntegral Filtro { get; set; }
    }

    public class ObterEducacaoIntegralQueryValidator : AbstractValidator<ObterEducacaoIntegralQuery>
    {
        public ObterEducacaoIntegralQueryValidator()
        {
            RuleFor(c => c.Filtro.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
