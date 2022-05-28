using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioRaaEscolaAquiUseCase
    {
        Task<bool> Executar(FiltroRelatorioEscolaAquiDto relatorioBoletimEscolaAquiDto);
    }
}