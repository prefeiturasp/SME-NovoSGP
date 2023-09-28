using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAulaReposicaoConsulta
    {
        Task<long[]> ObterAulasReposicaoComPendenciaCriada(long[] aulasId);
    }
}
