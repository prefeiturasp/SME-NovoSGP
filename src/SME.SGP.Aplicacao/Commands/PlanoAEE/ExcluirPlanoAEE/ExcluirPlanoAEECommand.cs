using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEECommand : IRequest<bool>
    {
        public ExcluirPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ExcluirPlanoAEECommandValidator : AbstractValidator<ExcluirPlanoAEECommand>
    {
        public ExcluirPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O Id do Plano AEE deve ser informado para exclusão.");
        }
    }
}
