using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotaisAdesaoAgrupadosPorDreUseCase
    {
        Task<IEnumerable<TotaisAdesaoAgrupadoProDreResultado>> Executar();
    }

}