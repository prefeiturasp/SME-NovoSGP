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
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;

        public ConsultasPeriodoFechamento(IServicoPeriodoFechamento servicoPeriodoFechamento,
                                IRepositorioTurmaConsulta repositorioTurma,
                                IRepositorioUeConsulta repositorioUe,
                                IRepositorioDreConsulta repositorioDre,
                                IConsultasTipoCalendario consultasTipoCalendario,
                                IRepositorioEventoFechamentoConsulta repositorioEventoFechamento,
                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
                                IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

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

        public async Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(string turmaCodigo, DateTime dataReferencia, int bimestre = 0)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            return await TurmaEmPeriodoDeFechamentoVigente(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, DateTime dataReferencia, int bimestre = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            return await TurmaEmPeriodoDeFechamento(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(Turma turma, DateTime dataReferencia, int bimestre = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            return await TurmaEmPeriodoDeFechamentoVigente(turma, tipoCalendario, dataReferencia, bimestre);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamentoAula(Turma turma, DateTime dataReferencia, int bimestre = 0, int bimestreAlteracao = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            var ueEmFechamento = await UeEmFechamento(tipoCalendario, turma.EhTurmaInfantil, bimestre, dataReferencia);

            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestreAlteracao, dataReferencia);
        }

        public async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0)
        {
            var ueEmFechamento = await UeEmFechamento(tipoCalendario, turma.EhTurmaInfantil, bimestre, dataReferencia);

            bool retorno = ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);
            return retorno;
        }

        private async Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0)
        {
            var periodoFechamentoBimestre = await UeEmFechamentoVigente(tipoCalendario, turma.EhTurmaInfantil, bimestre, dataReferencia);

            if (periodoFechamentoBimestre != null)
                return new PeriodoFechamentoVigenteDto() { PeriodoFechamentoInicio = periodoFechamentoBimestre.InicioDoFechamento, PeriodoFechamentoFim = periodoFechamentoBimestre.FinalDoFechamento };

            var periodoReabertura = await UeEmReaberturaDeFechamentoVigente(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);

            if (periodoReabertura != null)
                return new PeriodoFechamentoVigenteDto() { PeriodoFechamentoInicio = periodoReabertura.Inicio, PeriodoFechamentoFim = periodoReabertura.Fim };

            return null;
        }

        private async Task<bool> UeEmFechamento(TipoCalendario tipoCalendario, bool modalidadeEhInfantil, int bimestre, DateTime dataReferencia)
        {
            return await repositorioEventoFechamento.UeEmFechamento(dataReferencia, tipoCalendario.Id, modalidadeEhInfantil, bimestre);
        }

        private async Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(TipoCalendario tipoCalendario, bool modalidadeEhInfantil, int bimestre, DateTime dataReferencia)
        {
            return await repositorioEventoFechamento.UeEmFechamentoVigente(dataReferencia, tipoCalendario.Id, modalidadeEhInfantil, bimestre);
        }

        private async Task<FechamentoReabertura> UeEmReaberturaDeFechamentoVigente(TipoCalendario tipoCalendario, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            return await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            bimestre,
                                                            dataReferencia,
                                                            tipoCalendario.Id,
                                                            dreCodigo,
                                                            ueCodigo);
        }

        private async Task<bool> UeEmReaberturaDeFechamento(TipoCalendario tipoCalendario, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            var reaberturaPeriodo = await UeEmReaberturaDeFechamentoVigente(tipoCalendario, ueCodigo, dreCodigo, bimestre, dataReferencia);
            return reaberturaPeriodo != null;
        }
    }
}