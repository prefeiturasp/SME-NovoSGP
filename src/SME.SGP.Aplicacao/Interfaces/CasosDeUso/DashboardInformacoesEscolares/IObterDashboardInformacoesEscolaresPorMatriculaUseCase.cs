using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardInformacoesEscolaresPorMatriculaUseCase
    {
        Task<IEnumerable<GraficoBaseDto>> Executar(FiltroGraficoMatriculaDto filtro);
    }


}
