using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaServidor : IRepositorioBase<OcorrenciaServidor>
    {
        Task ExcluirPorOcorrenciaAsync(long idOcorrencia);
        Task<IEnumerable<OcorrenciaServidor>> ObterPorIdOcorrencia(long idOcorrencia);
        Task ExcluirPoIds(IEnumerable<long> ids);
    }
}
