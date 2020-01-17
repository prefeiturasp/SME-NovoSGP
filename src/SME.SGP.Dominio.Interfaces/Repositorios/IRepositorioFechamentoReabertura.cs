using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoReabertura : IRepositorioBase<FechamentoReabertura>
    {
        IEnumerable<PaginacaoResultadoDto<FechamentoReabertura>> Listar(long tipoCalendarioId, long dreId, long ueId, Paginacao paginacao);
    }
}