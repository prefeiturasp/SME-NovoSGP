using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase
    {
        Task<IEnumerable<NotificacaoBasicaDto>> Executar(bool tituloReduzido);
    }
}