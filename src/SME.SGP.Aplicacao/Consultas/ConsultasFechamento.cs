using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamento : IConsultasFechamento
    {
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoFechamento servicoFechamento;

        public ConsultasFechamento(IServicoFechamento servicoFechamento,
                                IRepositorioTurma repositorioTurma,
                                IRepositorioUe repositorioUe,
                                IRepositorioDre repositorioDre,
                                IRepositorioTipoCalendario repositorioTipoCalendario,
                                IRepositorioEvento repositorioEvento,
                                IRepositorioEventoFechamento repositorioEventoFechamento,
                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.servicoFechamento = servicoFechamento ?? throw new System.ArgumentNullException(nameof(servicoFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<IEnumerable<PeriodoEscolarDto>> ObterPeriodosEmAberto(long ueId)
            => await repositorioEventoFechamento.ObterPeriodosEmAberto(ueId, DateTime.Now.Date);

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto)
        {
            return await servicoFechamento.ObterPorTipoCalendarioDreEUe(fechamentoDto.TipoCalendarioId, fechamentoDto.DreId, fechamentoDto.UeId);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(string turmaCodigo, DateTime dataReferencia, int bimestre)
        {
            var turma = repositorioTurma.ObterPorCodigo(turmaCodigo);
            var ue = repositorioUe.ObterPorId(turma.UeId);
            var dre = repositorioDre.ObterPorId(ue.DreId);

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo
                    , turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio
                    , dataReferencia.Month <= 6 ? 1 : 2);

            var ueEmFechamento = await repositorioEventoFechamento.UeEmFechamento(dataReferencia, dre.CodigoDre, ue.CodigoUe, bimestre, tipoCalendario.Id);
            
            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario.Id, ue.CodigoUe, dre.CodigoDre, bimestre, dataReferencia);
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