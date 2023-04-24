using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRelatorioAnaliticoSondagemUseCase
    {
        Task<bool> Executar(FiltroRelatorioAnaliticoSondagemDto filtroRelatorioAnaliticoSondagemDto);
    }
}
