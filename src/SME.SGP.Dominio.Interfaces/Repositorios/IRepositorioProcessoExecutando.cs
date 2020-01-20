using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioProcessoExecutando
    {
        Task<ProcessoExecutando> ObterProcessoCalculoFrequencia(string turmaId, string disciplinaId);
        Task<long> SalvarAsync(ProcessoExecutando processo);
        void Remover(ProcessoExecutando processo);
    }
}
