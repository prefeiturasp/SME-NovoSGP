using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordo : IRepositorioBase<DiarioBordo>
    {
        Task<DiarioBordo> ObterPorAulaId(long aulaId, long componenteCurricularId);
        Task<DiarioBordo> ObterPorAulaIdRegistroExcluido(long aulaId);
        Task<bool> ExisteDiarioParaAula(long aulaId);

        Task ExcluirDiarioBordoDaAula(long aulaId);

        Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorPeriodoPaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao);

        Task<IEnumerable<Tuple<long, DateTime>>> ObterDatasPorIds(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim);

        Task AtualizaDiariosComDevolutivaId(long devolutivaId, IEnumerable<long> diariosBordoIds);

        Task<DateTime?> ObterDataDiarioSemDevolutivaPorTurmaComponente(string turmaCodigo, long componenteCurricularCodigo);

        Task<IEnumerable<long>> ObterIdsPorDevolutiva(long devolutivaId);

        Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorDevolutivaPaginado(long devolutivaId, Paginacao paginacao);

        Task ExcluirReferenciaDevolutiva(long devolutivaId);

        Task<DiarioBordo> ObterDiarioBordoComAulaETurmaPorCodigo(long diarioBordoId);

        Task<PaginacaoResultadoDto<DiarioBordoResumoDto>> ObterListagemDiarioBordoPorPeriodoPaginado(long turmaId, string componenteCurricularPaiCodigo, long componenteCurricularFilhoCodigo, DateTime? periodoInicio, DateTime? periodoFim, Paginacao paginacao);

        Task <IEnumerable<QuantidadeTotalDiariosEDevolutivasPorAnoETurmaDTO>>ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade);

        Task<IEnumerable<QuantidadeTotalDiariosPendentesPorAnoETurmaDTO>> ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade);

        Task<IEnumerable<QuantidadeDiariosDeBordoComDevolutivaEDevolutivaPendentePorTurmaAnoDto>> ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteAsync(int anoLetivo, Modalidade modalidade, DateTime dataAula, long? dreId, long? ueId);

        Task<IEnumerable<DiarioBordo>> ObterIdDiarioBordoAulasExcluidas(string codigoTurma, string codigoDisciplina, long tipoCalendarioId, DateTime[] datasConsideradas);
    }
}