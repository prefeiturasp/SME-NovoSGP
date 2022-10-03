﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoReabertura : IRepositorioBase<FechamentoReabertura>
    {
        Task<FechamentoReabertura> ObterPorDataTurmaCalendarioAsync(long ueId, DateTime dataReferencia, long tipoCalendarioId);
        void ExcluirBimestres(long id);
        void ExcluirBimestre(long fechamentoReaberturaId, long bimestreId);

        Task ExcluirVinculoDeNotificacoesAsync(long fechamentoReaberturaId);

        Task<IEnumerable<FechamentoReabertura>> Listar(long tipoCalendarioId, long? dreId, long? ueId, long[] ids);

        Task<PaginacaoResultadoDto<FechamentoReabertura>> ListarPaginado(long tipoCalendarioId, string dreCodigo, string ueCodigo, Paginacao paginacao);

        FechamentoReabertura ObterCompleto(long idFechamentoReabertura = 0, long workflowId = 0);

        Task<IEnumerable<FechamentoReaberturaNotificacao>> ObterNotificacoes(long id);

        Task SalvarBimestreAsync(FechamentoReaberturaBimestre fechamentoReabertura);

        Task SalvarNotificacaoAsync(FechamentoReaberturaNotificacao fechamentoReaberturaNotificacao);

        Task<IEnumerable<FechamentoReabertura>> ObterReaberturaFechamentoBimestre(int bimestre, DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, string dreCodigo, string ueCodigo);

        Task<FechamentoReabertura> ObterReaberturaFechamentoBimestrePorDataReferencia(int bimestre, DateTime dataReferencia, long tipoCalendarioId, string dreCodigo, string ueCodigo);
        Task<IEnumerable<FechamentoReabertura>> ObterPorIds(long[] ids);
    }
}