using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe
{
    public class ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery : IRequest<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>>
    {
        public ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
    public class ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQueryValidator : AbstractValidator<ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQuery>
    {
        public ObterDadosParaConsolidarFluenciaLeitoraUePainelEducacionalQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}
