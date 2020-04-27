using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConselhoClasseParecerConclusivo : IRepositorioBase<ConselhoClasseParecerConclusivo>
    {
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaCodigo(long turmaCodigo, DateTime dataConsulta);
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaId(long turmaId, DateTime dataConsulta);
    }
}
