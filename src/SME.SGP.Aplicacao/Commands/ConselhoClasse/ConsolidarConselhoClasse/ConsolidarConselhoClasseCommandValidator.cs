using FluentValidation;

namespace SME.SGP.Aplicacao
{

    public class ConsolidarConselhoClasseCommandValidator : AbstractValidator<ConsolidarConselhoClasseCommand>
    {
        public ConsolidarConselhoClasseCommandValidator()
        {
            RuleFor(c => c.DreId)
                .NotNull()
                .WithMessage("O código da DRE deve ser informado para consolidar conselho de classe.");
        }
    }
}