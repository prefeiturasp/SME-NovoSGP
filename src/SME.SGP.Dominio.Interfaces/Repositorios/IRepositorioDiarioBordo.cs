﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordo : IRepositorioBase<DiarioBordo>
    {
        Task<DiarioBordo> ObterPorAulaId(long aulaId);
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

        Task<PaginacaoResultadoDto<DiarioBordoResumoDto>> ObterListagemDiarioBordoPorPeriodoPaginado(long turmaId, long componenteCurricularCodigo, DateTime? periodoInicio, DateTime? periodoFim, Paginacao paginacao);
    }
}
