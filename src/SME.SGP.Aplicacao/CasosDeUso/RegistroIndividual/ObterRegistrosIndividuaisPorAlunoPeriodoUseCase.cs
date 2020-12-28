using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorAlunoPeriodoUseCase : IObterRegistrosIndividuaisPorAlunoPeriodoUseCase
    {
        public async Task<IEnumerable<RegistroIndividualDataDto>> Executar(FiltroRegistroIndividualAlunoPeriodo param)
        {
            return await Task.FromResult(Enumerable.Empty<RegistroIndividualDataDto>());
        }
    }
}
