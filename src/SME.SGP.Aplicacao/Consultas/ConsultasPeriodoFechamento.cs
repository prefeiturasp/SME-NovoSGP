﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPeriodoFechamento : IConsultasPeriodoFechamento
    {
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;

        public ConsultasPeriodoFechamento(IServicoPeriodoFechamento servicoPeriodoFechamento,
                                IRepositorioTurma repositorioTurma,
                                IRepositorioUe repositorioUe,
                                IRepositorioDre repositorioDre,
                                IConsultasTipoCalendario consultasTipoCalendario,
                                IRepositorioEvento repositorioEvento,
                                IRepositorioEventoFechamento repositorioEventoFechamento,
                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<IEnumerable<PeriodoEscolarDto>> ObterPeriodosEmAberto(long ueId)
            => await repositorioEventoFechamento.ObterPeriodosEmAberto(ueId, DateTime.Now.Date);

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto)
        {
            return await servicoPeriodoFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto.TipoCalendarioId, fechamentoDto.DreId, fechamentoDto.UeId);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(string turmaCodigo, DateTime dataReferencia, int bimestre = 0)
        {
            var turma = repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma, dataReferencia);

            return await TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, DateTime dataReferencia, int bimestre = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma, dataReferencia);

            return await TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0)
        {
            if (turma.Ue == null)
                turma.AdicionarUe(repositorioUe.ObterPorId(turma.UeId));
            if (turma.Ue.Dre == null)
                turma.Ue.AdicionarDre(repositorioDre.ObterPorId(turma.Ue.DreId));

            var ueEmFechamento = await repositorioEventoFechamento.UeEmFechamento(dataReferencia, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, tipoCalendario.Id, bimestre);
            
            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario.Id, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);
        }

        private async Task<bool> UeEmReaberturaDeFechamento(long tipoCalendarioId, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            // Busca eventos de fechamento na data atual
            var eventosFechamento = await repositorioEvento.EventosNosDiasETipo(dataReferencia, dataReferencia,
                                            TipoEvento.FechamentoBimestre, tipoCalendarioId, ueCodigo, dreCodigo);

            foreach (var eventoFechamento in eventosFechamento)
            {
                // Verifica existencia de reabertura de fechamento com mesmo inicio e fim do evento de fechamento
                var reaberturasPeriodo = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestre(
                                                                bimestre,
                                                                eventoFechamento.DataInicio,
                                                                eventoFechamento.DataFim,
                                                                tipoCalendarioId,
                                                                dreCodigo,
                                                                ueCodigo);

                if (reaberturasPeriodo != null && reaberturasPeriodo.Any())
                    return true;
            }

            return false;
        }
    }
}