using FluentValidation;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdepParaConsolidacao
{
    public class ObterProficienciaIdepParaConsolidacaoQueryValidator : AbstractValidator<ObterProficienciaIdepParaConsolidacaoQuery>
    {
        public ObterProficienciaIdepParaConsolidacaoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}