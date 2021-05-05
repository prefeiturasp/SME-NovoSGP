using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardFrequenciaPorAnoUseCase 
    {
        Task<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>> Executar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre);
    }
    

}
