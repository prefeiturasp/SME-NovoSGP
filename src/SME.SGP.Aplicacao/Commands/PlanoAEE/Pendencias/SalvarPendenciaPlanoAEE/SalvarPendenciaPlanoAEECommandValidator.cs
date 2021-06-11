using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaPlanoAEECommandValidator : AbstractValidator<SalvarPendenciaPlanoAEECommand>
    {
        public SalvarPendenciaPlanoAEECommandValidator()
        {
            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O id da pendencia deve ser informado.");

            RuleFor(c => c.PlanoAEEId)
            .NotEmpty()
            .WithMessage("O id do PlanoAEE deve ser informado.");
        }

    }

}
