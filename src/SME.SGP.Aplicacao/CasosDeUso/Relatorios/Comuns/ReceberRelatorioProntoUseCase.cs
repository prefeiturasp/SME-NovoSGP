using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{


    public class ReceberRelatorioProntoUseCase : IReceberRelatorioProntoUseCase, IUseCase<DadosRelatorioDto, bool>
    {
        public Task<bool> Executar(DadosRelatorioDto param)
        {
            throw new System.NotImplementedException();
        }
    }
}
