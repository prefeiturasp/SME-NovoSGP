using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterListaAtividadeMuralUseCase
    {
       Task<IEnumerable<object>> BuscarPorAulaId(long aulaId);
    }
}
