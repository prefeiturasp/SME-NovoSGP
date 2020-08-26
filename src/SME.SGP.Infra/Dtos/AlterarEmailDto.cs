using FluentValidation;

namespace SME.SGP.Infra
{
    public class AlterarEmailDto
    {
        public string NovoEmail { get; set; }
    }

    public class AlterarEmailDtoValidator : AbstractValidator<AlterarEmailDto>
    {

        public AlterarEmailDtoValidator()
        {
            RuleFor(c => c.NovoEmail)
                .NotEmpty()
                .WithMessage("O novo e-mail deve ser informado.")
                .EmailAddress()
                .WithMessage("E-mail inválido.");
        }
    }
}