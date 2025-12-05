using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IReabrirAtendimentoNAAPAUseCase
    {
        Task<SituacaoDto> Executar(long encaminhamentoId);
    }
}
