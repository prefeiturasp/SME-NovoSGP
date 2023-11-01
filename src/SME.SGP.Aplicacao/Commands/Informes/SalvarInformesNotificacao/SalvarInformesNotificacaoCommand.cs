using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesNotificacaoCommand : IRequest<long>
    {
        public SalvarInformesNotificacaoCommand(long informeId, long notificacaoId)
        {
            InformeId = informeId;
            NotificacaoId = notificacaoId;
        }

        public long InformeId { get; set; }
        public long NotificacaoId { get; set; }
    }

    public class SalvarInformesNotificacaoCommandValidator : AbstractValidator<SalvarInformesNotificacaoCommand>
    {
        public SalvarInformesNotificacaoCommandValidator()
        {
            RuleFor(a => a.InformeId)
               .NotEmpty()
               .WithMessage("O id do informativo deve ser informado para o cadastro do vínculo com a notificação.");

            RuleFor(a => a.NotificacaoId)
               .NotEmpty()
               .WithMessage("O id da notificação deve ser informado para o cadastro do vínculo com a notificação.");
        }
    }
}
