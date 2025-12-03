using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarObservacoesDeEncaminhamentoNAAPAUseCase
    {
        Task<bool> Executar(AtendimentoNAAPAObservacaoSalvarDto filtro);
    }
}