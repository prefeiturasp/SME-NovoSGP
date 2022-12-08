using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoPorIdQuery : IRequest<Notificacao>
    {
        public ObterNotificacaoPorIdQuery(long notificacaoId)
        {
            NotificacaoId = notificacaoId;
        }

        public long NotificacaoId { get; }
    }
}