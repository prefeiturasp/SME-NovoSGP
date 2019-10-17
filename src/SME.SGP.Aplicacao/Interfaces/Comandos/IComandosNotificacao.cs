using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IComandosNotificacao
    {
        List<AlteracaoStatusNotificacaoDto> Excluir(IList<long> notificacoesId);

        List<AlteracaoStatusNotificacaoDto> MarcarComoLida(IList<long> notificacoesId);

        void Salvar(NotificacaoDto notificacaoDto);
    }
}