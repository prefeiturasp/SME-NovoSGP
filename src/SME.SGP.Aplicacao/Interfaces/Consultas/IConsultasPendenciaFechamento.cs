using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPendenciaFechamento
    {
        Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> Listar(FiltroPendenciasFechamentosDto filtro);
        Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId);
    }
}
