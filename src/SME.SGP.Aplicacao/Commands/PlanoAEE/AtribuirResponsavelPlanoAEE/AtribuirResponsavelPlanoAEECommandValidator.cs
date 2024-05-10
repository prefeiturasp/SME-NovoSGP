using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommandValidator : AbstractValidator<AtribuirResponsavelPlanoAEECommand>
    {
        public AtribuirResponsavelPlanoAEECommandValidator()
        {
            RuleFor(x => x.PlanoAEE)
                .NotNull()
                .WithMessage("O Plano AEE deve ser informado para atribuição de responsável!");
            RuleFor(x => x.ResponsavelRF)
                   .NotEmpty()
                   .WithMessage("O RF do responsável deve ser informado para atribuição de responsável!");
        }
    }
}
