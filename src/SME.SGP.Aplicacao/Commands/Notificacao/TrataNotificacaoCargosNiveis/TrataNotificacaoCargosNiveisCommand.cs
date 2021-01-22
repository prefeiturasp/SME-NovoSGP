using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacaoCargosNiveisCommand : IRequest<bool>
    {
        public TrataNotificacaoCargosNiveisCommand(IEnumerable<NotificacoesParaTratamentoCargosNiveisDto> notificacoes)
        {
            Notificacoes = notificacoes;
        }

        public IEnumerable<NotificacoesParaTratamentoCargosNiveisDto> Notificacoes { get; set; }
    }
}
