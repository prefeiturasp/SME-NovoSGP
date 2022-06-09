using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExisteResponsavelAtribuidoUePorUeTipoQueryValidator : AbstractValidator<ExisteResponsavelAtribuidoUePorUeTipoQuery>
    {
        public ExisteResponsavelAtribuidoUePorUeTipoQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");

            RuleFor(c => c.TipoResponsavelAtribuicao)
                .IsInEnum()
                .WithMessage("O tipo de responsável atribuição deve ser informado.");
        }
    }
}
