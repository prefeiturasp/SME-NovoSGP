using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualUseCase : IObterRegistroIndividualUseCase
    {
        public async Task<RegistroIndividualDto> Executar(long param)
        {
            return await Task.FromResult(new RegistroIndividualDto());
        }
    }
}
