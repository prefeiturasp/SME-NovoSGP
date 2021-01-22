using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IListarAlunosDaTurmaRegistroIndividualUseCase : IUseCase<FiltroRegistroIndividualBase, IEnumerable<AlunoDadosBasicosDto>>
    {
    }
}
