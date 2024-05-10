using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAulaPorIdUseCase
    {
        Task<AulaConsultaDto> Executar(long aulaId);
    }
}
