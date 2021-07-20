using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoNotificacaoPorIdQuery : IRequest<long>
    {
        public ObterCodigoNotificacaoPorIdQuery(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }

        public long NotificacaoId { get; }
    }

    public class ObterCodigoNotificacaoPorIdQueryValidator : AbstractValidator<ObterCodigoNotificacaoPorIdQuery>
    {
        public ObterCodigoNotificacaoPorIdQueryValidator()
        {
            RuleFor(a => a.NotificacaoId)
                .NotEmpty()
                .WithMessage("O id da notificação deve ser informado para consulta de seu código");
        }
    }
}
