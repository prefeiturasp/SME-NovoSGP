using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroIndividualUseCase : IAlterarRegistroIndividualUseCase
    {
        public async Task<AuditoriaDto> Executar(AlterarRegistroIndividualDto param)
        {
            return await Task.FromResult(new AuditoriaDto());
        }
    }
}
