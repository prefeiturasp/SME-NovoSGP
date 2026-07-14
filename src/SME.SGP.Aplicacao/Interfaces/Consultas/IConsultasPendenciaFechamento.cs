using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPendenciaFechamento
    {
        Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> Listar(FiltroPendenciasFechamentosDto filtro);
        Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId);
    }
}
