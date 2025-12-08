using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarObservacoesDeAtendimentoNAAPAUseCase
    {
        Task<bool> Executar(AtendimentoNAAPAObservacaoSalvarDto filtro);
    }
}