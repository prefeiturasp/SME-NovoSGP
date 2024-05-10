using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class CadastrarParecerPAAICommandValidator : AbstractValidator<CadastrarParecerPAAICommand>
    {
        public CadastrarParecerPAAICommandValidator()
        {
            RuleFor(x => x.PlanoAEEId)
                   .GreaterThan(0)
                   .WithMessage("O PlanoAEE deve ser informado!");

            RuleFor(x => x.ParecerPAAI)
                    .NotEmpty()
                    .WithMessage("O parecer do PAAI deve ser informado!");
        }
    }
}
