using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaFechamentoAulaConsulta
    {
        Task<IEnumerable<long>> ObterIdsAulaDaPendenciaDeFechamento(IEnumerable<long> idsPendenciaFechamento);
        Task<IEnumerable<PendenciaFechamento>> ObterPendenciaFechamentoDeAula(long idAula, TipoPendencia tipoPendencia);
    }
}
