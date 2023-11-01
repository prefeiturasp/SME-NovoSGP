using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformativoNotificacaoCommand : IRequest<long>
    {
        public SalvarInformativoNotificacaoCommand(long informativoId, long notificacaoId)
        {
            InformativoId = informativoId;
            NotificacaoId = notificacaoId;
        }

        public long InformativoId { get; set; }
        public long NotificacaoId { get; set; }
    }

    public class SalvarInformesNotificacaoCommandValidator : AbstractValidator<SalvarInformativoNotificacaoCommand>
    {
        public SalvarInformesNotificacaoCommandValidator()
        {
            RuleFor(a => a.InformativoId)
               .NotEmpty()
               .WithMessage("O id do informativo deve ser informado para o cadastro do vínculo com a notificação.");

            RuleFor(a => a.NotificacaoId)
               .NotEmpty()
               .WithMessage("O id da notificação deve ser informado para o cadastro do vínculo com a notificação.");
        }
    }
}
