using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterMuralAvisosUseCase
    {
        Task<IEnumerable<MuralAvisosRetornoDto>> BuscarPorAulaId(long aulaId);
    }
}