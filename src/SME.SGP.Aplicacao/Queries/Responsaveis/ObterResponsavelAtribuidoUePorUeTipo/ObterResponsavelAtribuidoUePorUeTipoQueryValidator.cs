using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsavelAtribuidoUePorUeTipoQueryValidator : AbstractValidator<ObterResponsavelAtribuidoUePorUeTipoQuery>
    {
        public ObterResponsavelAtribuidoUePorUeTipoQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado para obter responsáveis atribuídos à escola.");

            RuleFor(c => c.TipoResponsavelAtribuicao)
                .IsInEnum()
                .WithMessage("O tipo de responsável atribuição deve ser informado.");
        }
    }
}
