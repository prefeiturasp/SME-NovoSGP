using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterUltimaAtualizacaoPorProcessoUseCase
    {
        Task<UltimaAtualizaoWorkerPorProcessoResultado> Executar(string nomeProcesso);
    }
}