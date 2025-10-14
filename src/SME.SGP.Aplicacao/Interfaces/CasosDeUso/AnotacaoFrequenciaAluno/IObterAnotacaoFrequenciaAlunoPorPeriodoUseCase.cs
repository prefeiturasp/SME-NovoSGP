using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterAnotacaoFrequenciaAlunoPorPeriodoUseCase : IUseCase<FiltroAnotacaoFrequenciaAlunoPorPeriodoDto, IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>>
    {
    }
}
