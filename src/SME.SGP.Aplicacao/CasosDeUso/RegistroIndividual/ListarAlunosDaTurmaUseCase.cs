using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosDaTurmaUseCase : IListarAlunosDaTurmaUseCase
    {
        public async Task<IEnumerable<AlunoDadosBasicosDto>> Executar(FiltroRegistroIndividualBase param)
        {
            return await Task.FromResult(Enumerable.Empty<AlunoDadosBasicosDto>());
        }
    }
}
