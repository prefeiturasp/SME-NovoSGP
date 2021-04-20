using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPlanoAEEObservacaoCommand : IRequest<bool>
    {
        public ExcluirNotificacaoPlanoAEEObservacaoCommand(long observacaoPlanoId)
        {
            ObservacaoPlanoId = observacaoPlanoId;
        }

        public long ObservacaoPlanoId { get; }
    }

    public class ExcluirNotificacaoPlanoAEEObservacaoCommandValidator : AbstractValidator<ExcluirNotificacaoPlanoAEEObservacaoCommand>
    {
        public ExcluirNotificacaoPlanoAEEObservacaoCommandValidator()
        {
            RuleFor(a => a.ObservacaoPlanoId)
                .NotEmpty()
                .WithMessage("O id da Observação do plano AEE deve ser informado para exclusão de suas notificações");
        }
    }
}
