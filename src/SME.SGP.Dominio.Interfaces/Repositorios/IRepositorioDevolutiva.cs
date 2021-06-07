using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDevolutiva : IRepositorioBase<Devolutiva>
    {
        Task<DateTime> ObterUltimaDataDevolutiva(string turmaCodigo, long componenteCurricularCodigo);
        Task<PaginacaoResultadoDto<DevolutivaResumoDto>> ListarDevolutivasPorTurmaComponentePaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime? dataReferencia, Paginacao paginacao);
        Task<IEnumerable<long>> ObterDevolutivasPorTurmaComponenteNoPeriodo(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio1, DateTime periodoInicio2);
        Task<ConsolidacaoDevolutivaTurmaDTO> ObterDevolutivasPorTurma(string turmaCodigo);
    }
}
