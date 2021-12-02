using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodoPorComponenteUseCase
    {
        Task<List<PeriodoEscolarComponenteDto>> Executar(string turmaCodigo, string componenteCodigo, int bimestre);
    }
}
