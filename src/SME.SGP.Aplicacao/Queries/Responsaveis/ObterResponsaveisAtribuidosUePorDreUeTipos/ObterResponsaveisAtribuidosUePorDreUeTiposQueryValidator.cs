using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUePorDreUeTiposQueryValidator : AbstractValidator<ObterResponsaveisAtribuidosUePorDreUeTiposQuery>
    {
        public ObterResponsaveisAtribuidosUePorDreUeTiposQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .When(a => string.IsNullOrEmpty(a.CodigoDre))
                .WithMessage("O código da Ue deve ser informado para obter responsáveis atribuídos à escola.");
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .When(a => string.IsNullOrEmpty(a.CodigoUe))
                .WithMessage("O código da Dre deve ser informado para obter responsáveis atribuídos à escola.");
            RuleFor(c => c.TiposResponsavelAtribuicao)
                .NotEmpty()
                .WithMessage("Os tipos de responsável atribuição devem ser informados.");
        }
    }
}
