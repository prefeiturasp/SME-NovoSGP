using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorIdQueryValidator : AbstractValidator<ObterUeComDrePorIdQuery>
    {
        public ObterUeComDrePorIdQueryValidator()
        {
            RuleFor(c => c.UeId)
            .NotEmpty()
            .WithMessage("O ID da Ue deve ser informado.");
        }
    }
}
