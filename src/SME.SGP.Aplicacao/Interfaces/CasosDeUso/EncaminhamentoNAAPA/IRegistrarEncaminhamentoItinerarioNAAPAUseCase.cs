using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarEncaminhamentoItinerarioNAAPAUseCase
    {
        Task<bool> Executar(AtendimentoNAAPAItineranciaDto encaminhamentoNAAPAItineranciaDto);
    }
}
