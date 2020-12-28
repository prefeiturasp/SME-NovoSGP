using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualUseCase : IInserirRegistroIndividualUseCase
    {
        public async Task<AuditoriaDto> Executar(InserirRegistroIndividualDto param)
        {
            return await Task.FromResult(new AuditoriaDto());
        }
    }
}
