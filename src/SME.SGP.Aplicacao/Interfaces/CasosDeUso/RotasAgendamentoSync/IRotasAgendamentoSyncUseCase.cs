using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRotasAgendamentoSyncUseCase
    {
        Task<bool> Executar();
    }
}
