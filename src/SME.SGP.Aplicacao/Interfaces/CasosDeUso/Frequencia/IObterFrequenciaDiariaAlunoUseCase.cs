using SME.SGP.Infra;
using System.Collections.Generic;


namespace SME.SGP.Aplicacao
{
    public interface IObterFrequenciaDiariaAlunoUseCase : IUseCase<FiltroFrequenciaDiariaAlunoDto, PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
    }
}
