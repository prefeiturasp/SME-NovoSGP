using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IReabrirEncaminhamentoNAAPAUseCase
    {
        Task<SituacaoDto> Executar(long encaminhamentoId);
    }
}
