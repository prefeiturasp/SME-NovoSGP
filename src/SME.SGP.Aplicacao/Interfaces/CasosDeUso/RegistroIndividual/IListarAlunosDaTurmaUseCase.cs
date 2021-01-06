using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IListarAlunosDaTurmaUseCase : IUseCase<FiltroRegistroIndividualBase, IEnumerable<AlunoDadosBasicosDto>>
    {
    }
}
