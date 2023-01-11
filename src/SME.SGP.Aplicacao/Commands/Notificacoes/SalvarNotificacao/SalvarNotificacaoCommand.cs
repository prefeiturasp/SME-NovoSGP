using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarNotificacaoCommand : IRequest<long>
    {
        public SalvarNotificacaoCommand(Notificacao notificacao)
        {
            Notificacao = notificacao;
        }

        public Notificacao Notificacao { get; }
    }
}