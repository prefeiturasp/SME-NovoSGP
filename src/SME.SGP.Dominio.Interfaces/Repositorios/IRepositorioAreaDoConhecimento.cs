using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioAreaDoConhecimento
    {
        Task<IEnumerable<AreaDoConhecimentoDto>> ObterAreasDoConhecimentoPorComponentesCurriculares(long[] codigosComponentesCurriculares);
    }
}
