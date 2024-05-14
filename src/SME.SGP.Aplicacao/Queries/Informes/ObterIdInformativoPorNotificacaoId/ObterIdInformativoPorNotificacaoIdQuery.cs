using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdInformativoPorNotificacaoIdQuery : IRequest<long>
    {
        public ObterIdInformativoPorNotificacaoIdQuery(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }
        public long NotificacaoId { get; }
    }

    public class ObterIdInformativoPorNotificacaoIdQueryValidator : AbstractValidator<ObterIdInformativoPorNotificacaoIdQuery>
    {
        public ObterIdInformativoPorNotificacaoIdQueryValidator()
        {
            RuleFor(c => c.NotificacaoId)
                .GreaterThan(0)
                .WithMessage("O Id da notificação deve ser informado para a pesquisa.");
        }
    }
}