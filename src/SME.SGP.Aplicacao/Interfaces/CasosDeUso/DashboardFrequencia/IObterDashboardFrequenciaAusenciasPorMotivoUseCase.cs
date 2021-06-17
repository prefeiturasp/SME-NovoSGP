using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterDashboardFrequenciaAusenciasPorMotivoUseCase
    {
        Task<IEnumerable<GraficoBaseDto>> Executar(int anoLetivo, long dreId, long ueId, Modalidade? modalidade = null, string ano = "", long turmaId = 0, int semestre = 0);
    }
}
