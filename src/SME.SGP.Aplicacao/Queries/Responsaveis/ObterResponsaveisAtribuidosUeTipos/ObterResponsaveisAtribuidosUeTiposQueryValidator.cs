using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUeTiposQueryValidator : AbstractValidator<ObterResponsaveisAtribuidosUeTiposQuery>
    {
        public ObterResponsaveisAtribuidosUeTiposQueryValidator()
        {
            RuleFor(c => c.TiposResponsavelAtribuicao)
                .NotEmpty()
                .WithMessage("Os tipos de responsável atribuição devem ser informados.");
        }
    }
}
