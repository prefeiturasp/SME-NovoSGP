using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioProcessoExecutando : IRepositorioBase<ProcessoExecutando>
    {
        Task<ProcessoExecutando> ObterProcessoCalculoFrequencia(string turmaId, string disciplinaId);
    }
}
