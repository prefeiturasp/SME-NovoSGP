using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterMuralAvisosUseCase
    {
        Task<IEnumerable<MuralAvisosRetornoDto>> BuscarPorAulaId(long aulaId);
    }
}