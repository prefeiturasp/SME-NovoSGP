using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterJustificativasAlunoPorComponenteCurricularUseCase : IUseCase<FiltroJustificativasAlunoPorComponenteCurricular, PaginacaoResultadoDto<JustificativaAlunoDto>>
    {
    }
}
