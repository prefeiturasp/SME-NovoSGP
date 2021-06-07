using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioProcessoExecutando
    {
        Task<long> SalvarAsync(ProcessoExecutando processo);
        void Remover(ProcessoExecutando processo);
        Task RemoverAsync(ProcessoExecutando processo);
        Task RemoverIdsAsync(long[] ids);
        Task<ProcessoExecutando> ObterProcessoCalculoFrequenciaAsync(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso);
        Task<IEnumerable<ProcessoExecutando>> ObterProcessosEmExecucaoAsync(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso);
        Task<bool> ObterAulaEmManutencaoAsync(long aulaId);
        Task<IEnumerable<long>> ObterIdsPorFiltrosAsync(int bimestre, string disciplinaId, string turmaId);
        Task RemoverPorId(long id);
        Task<bool> ProcessoEstaEmExecucao(TipoProcesso tipoProcesso);
    }
}
