using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaEncaminhamentoAEECommandValidator : AbstractValidator<ExcluirPendenciaEncaminhamentoAEECommand>
    {
        public ExcluirPendenciaEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.PendenciaId)
                .NotEmpty()
                .WithMessage("O id da pendência deve ser informado.");
        }
    }
}
