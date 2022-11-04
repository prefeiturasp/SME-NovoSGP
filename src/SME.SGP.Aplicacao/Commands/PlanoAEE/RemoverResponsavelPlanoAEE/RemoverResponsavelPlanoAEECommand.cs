using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelPlanoAEECommand : IRequest<bool>
    {
        public RemoverResponsavelPlanoAEECommand(long planoAeeId)
        {
            PlanoAeeId = planoAeeId;
        }

        public long PlanoAeeId { get; set; }
    }

    public class RemoverResponsavelPlanoAEECommandValidator : AbstractValidator<RemoverResponsavelPlanoAEECommand>
    {
        public RemoverResponsavelPlanoAEECommandValidator()
        {
            RuleFor(x => x.PlanoAeeId)
                .GreaterThan(0)
                .WithMessage("O Id do Plano AEE deve ser informado para a remoção da atribuição de responsável!");
        }
    }
}