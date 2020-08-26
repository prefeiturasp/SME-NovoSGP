using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioProcessoExecutando
    {
        Task<long> SalvarAsync(ProcessoExecutando processo);
        void Remover(ProcessoExecutando processo);
        Task RemoverAsync(ProcessoExecutando processo);
        Task RemoverIdsAsync(long[] ids);
        Task<ProcessoExecutando> ObterProcessoCalculoFrequencia(string turmaId, string disciplinaId, int bimestre);
        Task<bool> ObterAulaEmManutencaoAsync(long aulaId);
    }
}
