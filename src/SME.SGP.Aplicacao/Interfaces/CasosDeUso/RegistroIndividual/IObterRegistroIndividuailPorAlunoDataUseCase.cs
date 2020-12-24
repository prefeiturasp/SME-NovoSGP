using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterRegistroIndividuailPorAlunoDataUseCase : IUseCase<FiltroRegistroIndividualAlunoData, IEnumerable<AlunoDadosBasicosDto>>
    {
    }
}
