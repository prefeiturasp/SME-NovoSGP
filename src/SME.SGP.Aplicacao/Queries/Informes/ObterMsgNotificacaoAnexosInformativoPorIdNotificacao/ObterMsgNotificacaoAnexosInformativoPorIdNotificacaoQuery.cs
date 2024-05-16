using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery : IRequest<string>
    {
        public ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }
        public long NotificacaoId { get; }
    }

    public class ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQueryValidator : AbstractValidator<ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQuery>
    {
        public ObterMsgNotificacaoAnexosInformativoPorIdNotificacaoQueryValidator()
        {
            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage("O Id da notificação deve ser informada para a busca dos anexos do informativo.");
        }
    }
}