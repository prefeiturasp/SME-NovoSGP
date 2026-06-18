using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterQuestionarioItinerarioEncaminhamentoNAAPAUseCase
    {
        Task<EncaminhamentoNAAPASecaoItineranciaQuestoesDto> Executar(long questionarioId, long? encaminhamentoSecaoId);
    }
}
