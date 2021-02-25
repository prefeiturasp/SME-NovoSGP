using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommandValidator : AbstractValidator<AtribuirResponsavelPlanoAEECommand>
    {
        public AtribuirResponsavelPlanoAEECommandValidator()
        {
            RuleFor(x => x.PlanoAEEId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Plano AEE deve ser informado!");
            RuleFor(x => x.ResponsavelRF)
                   .NotEmpty()
                   .WithMessage("O RF do responsável deve ser informado!");
        }
    }
}
