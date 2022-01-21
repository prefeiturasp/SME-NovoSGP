using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodosPorComponenteUseCase
    {
        Task<IEnumerable<PeriodoEscolarComponenteDto>> Executar(string turmaCodigo, long componenteCodigo, bool ehRegencia, int bimestre);
    }
}
