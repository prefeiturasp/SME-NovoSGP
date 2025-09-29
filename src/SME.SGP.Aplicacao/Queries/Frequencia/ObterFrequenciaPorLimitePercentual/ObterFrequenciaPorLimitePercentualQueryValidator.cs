using FluentValidation;
using SME.SGP.Infra.Consts;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
{
    public class ObterFrequenciaPorLimitePercentualQueryValidator : AbstractValidator<ObterFrequenciaPorLimitePercentualQuery>
    {
        public ObterFrequenciaPorLimitePercentualQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .WithMessage($"O ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
            RuleFor(c => c.LimitePercentual)
                .GreaterThan(0)
                .WithMessage("O limite percentual deve ser maior que 0.");
        }
    }   
}
