using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDatasDiarioBordoPorPeriodoUseCase
    {
        Task<IEnumerable<DiarioBordoPorPeriodoDto>> Executar(string turmaCodigo, DateTime dataInicio, DateTime dataFim, long componenteCurricularId);
    }
}
