using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreQueryValidator : AbstractValidator<ObterResponsaveisPorDreQuery>
    {
        public ObterResponsaveisPorDreQueryValidator()
        {
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");

            RuleFor(c => c.TipoResponsavelAtribuicao)
                .IsInEnum()
                .WithMessage("O tipo de responsável atribuição deve ser informado.");
        }
    }
}
