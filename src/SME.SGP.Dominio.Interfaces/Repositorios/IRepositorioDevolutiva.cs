using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDevolutiva : IRepositorioBase<Devolutiva>
    {
        Task<DateTime> ObterUltimaDataDevolutiva(string turmaCodigo, long componenteCurricularCodigo);
        Task<Devolutiva> ObterPorIdRegistroExcluido(long? devolutivaId);
        Task<PaginacaoResultadoDto<DevolutivaResumoDto>> ListarDevolutivasPorTurmaComponentePaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime? dataReferencia, Paginacao paginacao);
        Task<IEnumerable<long>> ObterDevolutivasPorTurmaComponenteNoPeriodo(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio1, DateTime periodoInicio2);
        Task<ConsolidacaoDevolutivaTurmaDTO> ObterDevolutivasPorTurmaEAnoLetivo(string turmaCodigo, int anoLetivo);

        Task<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO> ObterDiariosDeBordoPorTurmaEAnoLetivo(string turmaCodigo, int anoLetivo);

        Task<IEnumerable<DevolutivaTurmaDTO>> ObterTurmasInfantilComDevolutivasPorAno(int anoLetivo);
    }
}
