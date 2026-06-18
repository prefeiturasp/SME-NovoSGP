using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarObservacoesDeEncaminhamentoNAAPAUseCase
    {
        Task<bool> Executar(EncaminhamentoNAAPAObservacaoSalvarDto filtro);
    }
}