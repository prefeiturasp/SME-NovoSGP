using FluentValidation;
using SME.SGP.Infra.Consts;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap
{
    public class ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryValidator : AbstractValidator<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>
    {
        public ObterDadosBrutosParaConsolidacaoIndicadoresPapQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE).WithMessage($"O ano letivo deve ser maior ou igual {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
        }
    }
}
