using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarAtendimentoItinerarioNAAPAUseCase
    {
        Task<bool> Executar(AtendimentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto);
    }
}
