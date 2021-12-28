using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPeriodoFechamento : IConsultasPeriodoFechamento
    {
        private readonly IServicoPeriodoFechamento servicoPeriodoFechamento;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;

        public ConsultasPeriodoFechamento(IServicoPeriodoFechamento servicoPeriodoFechamento,
                                          IRepositorioTurma repositorioTurma,
                                          IConsultasTipoCalendario consultasTipoCalendario,
                                          IRepositorioEventoFechamento repositorioEventoFechamento,
                                          IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                          IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<PeriodoFechamentoBimestre> ObterPeriodoFechamentoTurmaAsync(Turma turma, int bimestre, long? periodoEscolarId)
            => await repositorioPeriodoFechamento.ObterPeriodoFechamentoTurma(turma.AnoLetivo, bimestre, periodoEscolarId);

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosComFechamentoEmAberto(long ueId)
            => await repositorioEventoFechamento.ObterPeriodosFechamentoEmAberto(ueId, DateTime.Now.Date);

        public async Task<FechamentoDto> ObterPorTipoCalendarioSme(FiltroFechamentoDto fechamentoDto)
        {
            return await servicoPeriodoFechamento.ObterPorTipoCalendarioSme(fechamentoDto.TipoCalendarioId);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(string turmaCodigo, DateTime dataReferencia, int bimestre = 0)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            return await TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, DateTime dataReferencia, int bimestre = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            return await TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamentoAula(Turma turma, DateTime dataReferencia, int bimestre = 0, int bimestreAlteracao = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
            var ueEmFechamento = await UeEmFechamento(tipoCalendario, bimestre, dataReferencia);
            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestreAlteracao, dataReferencia);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0)
        {
            bool modalidadeEhInfantil = turma.EhTurmaInfantil;

            var ueEmFechamento = await UeEmFechamento(tipoCalendario, modalidadeEhInfantil, bimestre, dataReferencia);

            bool retorno = ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);
            return retorno;
        }

        private async Task<bool> UeEmFechamento(TipoCalendario tipoCalendario, bool modalidadeEhInfantil, int bimestre, DateTime dataReferencia)
        {
            return await repositorioEventoFechamento.UeEmFechamento(tipoCalendario.Id, modalidadeEhInfantil, bimestre, dataReferencia);
        }

        private async Task<bool> UeEmReaberturaDeFechamento(TipoCalendario tipoCalendario, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            var reaberturaPeriodo = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            bimestre,
                                                            dataReferencia,
                                                            tipoCalendario.Id,
                                                            dreCodigo,
                                                            ueCodigo);
            return reaberturaPeriodo != null;
        }
    }
}