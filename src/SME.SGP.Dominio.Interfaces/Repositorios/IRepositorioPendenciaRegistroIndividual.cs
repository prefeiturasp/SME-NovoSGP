using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaRegistroIndividual : IRepositorioBase<PendenciaRegistroIndividual>
    {
        Task<PendenciaRegistroIndividual> ObterPendenciaRegistroIndividualPorTurmaESituacao(long turmaId, SituacaoPendencia situacaoPendencia);
        Task<IEnumerable<long>> ObterAlunosCodigosComPendenciaAtivosDaTurmaAsync(long turmaId);
    }
}