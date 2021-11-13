using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterListaAtividadeMuralUseCase
    {
       Task<IEnumerable<AtividadeInfantilDto>> BuscarPorAulaId(long aulaId);
    }
}
