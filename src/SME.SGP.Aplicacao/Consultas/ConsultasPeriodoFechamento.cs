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
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;

        public ConsultasPeriodoFechamento(IServicoPeriodoFechamento servicoPeriodoFechamento,
                                IConsultasTipoCalendario consultasTipoCalendario,
                                IRepositorioEventoFechamentoConsulta repositorioEventoFechamento,
                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.servicoPeriodoFechamento = servicoPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(servicoPeriodoFechamento));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosComFechamentoEmAberto(long ueId, int anoLetivo)
            => await repositorioEventoFechamento.ObterPeriodosFechamentoEmAberto(ueId, DateTime.Now.Date, anoLetivo);

        public async Task<FechamentoDto> ObterPorTipoCalendarioSme(FiltroFechamentoDto fechamentoDto)
        {
            return await servicoPeriodoFechamento.ObterPorTipoCalendarioSme(fechamentoDto.TipoCalendarioId, fechamentoDto.Aplicacao);
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

        public async Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoAnoAnterior(Turma turma, int bimestre = 0)
        {
            var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);

            var periodoFechamentoBimestre = await repositorioEventoFechamento.UeEmFechamentoBimestre(tipoCalendario.Id, turma.EhTurmaInfantil, bimestre);

            if (periodoFechamentoBimestre.NaoEhNulo())
                return new PeriodoFechamentoVigenteDto() { PeriodoFechamentoInicio = periodoFechamentoBimestre.InicioDoFechamento, PeriodoFechamentoFim = periodoFechamentoBimestre.FinalDoFechamento };

            return null;
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

            if (periodoFechamentoBimestre.NaoEhNulo())
                return new PeriodoFechamentoVigenteDto() { PeriodoFechamentoInicio = periodoFechamentoBimestre.InicioDoFechamento, PeriodoFechamentoFim = periodoFechamentoBimestre.FinalDoFechamento };

            var periodoReabertura = await UeEmReaberturaDeFechamentoVigente(tipoCalendario, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);

            if (periodoReabertura.NaoEhNulo())
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
            return reaberturaPeriodo.NaoEhNulo();
        }
    }
}