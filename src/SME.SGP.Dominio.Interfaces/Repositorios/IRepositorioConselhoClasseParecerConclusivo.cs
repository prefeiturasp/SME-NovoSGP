using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseParecerConclusivo : IRepositorioBase<ConselhoClasseParecerConclusivo>
    {
        Task<IEnumerable<ParecerConclusivoDto>> ObterListaResumidaPorTurmaCodigoAsync(string turmaCodigo, DateTime dataConsulta);
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaCodigoAsync(string turmaCodigo, DateTime dataConsulta);
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaIdAsync(long turmaId, DateTime dataConsulta);
    }
}
