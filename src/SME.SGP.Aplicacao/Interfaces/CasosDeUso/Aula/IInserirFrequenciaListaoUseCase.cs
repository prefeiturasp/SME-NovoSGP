using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IInserirFrequenciaListaoUseCase : IUseCase<IEnumerable<FrequenciaSalvarAulaAlunosDto>, FrequenciaAuditoriaDto>
    {
    }
}
