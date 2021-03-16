using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase : IUseCase<FiltroTurmaAlunoSemestreDto, IEnumerable<FrequenciaAlunoBimestreDto>>
    {
    }
}
