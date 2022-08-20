using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosNotificacao
    {
        List<AlteracaoStatusNotificacaoDto> Excluir(IList<long> notificacoesId);

        List<AlteracaoStatusNotificacaoDto> MarcarComoLida(IList<long> notificacoesId);

        Task Salvar(NotificacaoDto notificacaoDto);
    }
}