using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Notificacao.NotificacoesNiveisCargos
{
    public interface IExecutarTratamentoNotificacoesNiveisCargosUseCase
    {
        Task<bool> Executar(MensagemRabbit param);
    }
}
