using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidado
    {
        Task<IEnumerable<ConselhoClasseConsolidadoComponenteTurma>> ObterConselhosClasseConsolidadoPorTurmaBimestre(long turmaId, int bimestre);
    }
}
