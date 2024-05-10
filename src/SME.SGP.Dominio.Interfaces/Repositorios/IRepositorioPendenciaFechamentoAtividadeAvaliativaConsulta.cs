using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaFechamentoAtividadeAvaliativaConsulta
    {
        Task<IEnumerable<long>> ObterIdsAtividadeAvaliativaDaPendenciaDeFechamento(IEnumerable<long> idsPendenciaFechamento);
    }
}