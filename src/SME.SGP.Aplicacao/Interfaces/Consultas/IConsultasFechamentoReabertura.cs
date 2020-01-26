using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamentoReabertura
    {
        Task<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>> Listar(long tipoCalendarioId, string dreCodigo, string ueCodigo);

        FechamentoReaberturaRetornoDto ObterPorId(long id);
    }
}