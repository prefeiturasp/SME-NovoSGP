using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao
{
    public class ObterProficienciaIdebParaConsolidacaoQueryValidator : AbstractValidator<ObterProficienciaIdebParaConsolidacaoQuery>
    {
        public ObterProficienciaIdebParaConsolidacaoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}
