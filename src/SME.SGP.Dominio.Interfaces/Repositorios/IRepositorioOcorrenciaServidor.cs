using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaServidor : IRepositorioBase<OcorrenciaServidor>
    {
        Task ExcluirPorOcorrenciaAsync(long idOcorrencia);
    }
}
