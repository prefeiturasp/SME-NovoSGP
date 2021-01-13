using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaRegistroIndividual : IRepositorioBase<PendenciaRegistroIndividual>
    {
        Task<PendenciaRegistroIndividual> ObterPendenciaRegistroIndividualPorTurmaESituacao(long turmaId, SituacaoPendencia situacaoPendencia);
        Task<PendenciaRegistroIndividual> ObterPendenciaRegistroIndividualPorPendenciaESituacao(long pendenciaId, SituacaoPendencia situacaoPendencia,
            SituacaoPendenciaRegistroIndividualAluno situacaoAluno);
        Task<IEnumerable<long>> ObterAlunosCodigosComPendenciaAtivosDaTurmaAsync(long turmaId);
    }
}