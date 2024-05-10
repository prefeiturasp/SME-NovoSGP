using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaServidor : IRepositorioBase<OcorrenciaServidor>
    {
        Task ExcluirPorOcorrenciaAsync(long idOcorrencia);
        Task<IEnumerable<OcorrenciaServidor>> ObterPorIdOcorrencia(long idOcorrencia);
        Task ExcluirPoIds(IEnumerable<long> ids);
    }
}
