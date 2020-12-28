using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualPorAlunoDataUseCase : IObterRegistroIndividualPorAlunoDataUseCase
    {
        public async Task<RegistroIndividualDataDto> Executar(FiltroRegistroIndividualAlunoData param)
        {
            return await Task.FromResult(new RegistroIndividualDataDto());
        }
    }
}
