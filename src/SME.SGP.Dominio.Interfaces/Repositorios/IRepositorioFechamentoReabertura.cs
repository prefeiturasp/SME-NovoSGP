using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoReabertura : IRepositorioBase<FechamentoReabertura>
    {
        Task<PaginacaoResultadoDto<FechamentoReabertura>> Listar(long tipoCalendarioId, long? dreId, long? ueId, Paginacao paginacao);
    }
}