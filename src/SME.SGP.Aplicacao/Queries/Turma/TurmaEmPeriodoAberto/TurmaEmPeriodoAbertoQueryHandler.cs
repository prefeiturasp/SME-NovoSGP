using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoAbertoQueryHandler : IRequestHandler<TurmaEmPeriodoAbertoQuery, bool>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public TurmaEmPeriodoAbertoQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario,
                                                IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                IRepositorioEventoFechamento repositorioEventoFechamento,
        public TurmaEmPeriodoAbertoQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                IRepositorioEventoFechamentoConsulta repositorioEventoFechamento,
                                                IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<bool> Handle(TurmaEmPeriodoAbertoQuery request, CancellationToken cancellationToken)
        {
            long tipoCalendarioId;

            if (request.TipoCalendarioId == 0)
                tipoCalendarioId = await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.Turma.AnoLetivo
                                        , request.Turma.ModalidadeTipoCalendario
                                        , request.Turma.Semestre);
            else tipoCalendarioId = request.TipoCalendarioId;

            if (tipoCalendarioId == 0)
                throw new NegocioException($"Tipo de calendário para turma {request.Turma.CodigoTurma} não localizado!");

            var periodoEmAberto = await repositorioPeriodoEscolar.PeriodoEmAbertoAsync(tipoCalendarioId, request.DataReferencia, request.Bimestre, request.EhAnoLetivo);

            return periodoEmAberto || await TurmaEmPeriodoDeFechamento(request.Turma, tipoCalendarioId, request.DataReferencia, request.Bimestre);
        }

        private async Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, long tipoCalendarioId, DateTime dataReferencia, int bimestre)
        {
            var ueEmFechamento = await UeEmFechamento(turma, tipoCalendarioId, bimestre, dataReferencia);

            return ueEmFechamento || await UeEmReaberturaDeFechamento(tipoCalendarioId, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, bimestre, dataReferencia);
        }

        private async Task<bool> UeEmFechamento(Turma turma, long tipoCalendarioId, int bimestre, DateTime dataReferencia)
            => await repositorioEventoFechamento.UeEmFechamento(dataReferencia, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, tipoCalendarioId, bimestre);

        private async Task<bool> UeEmReaberturaDeFechamento(long tipoCalendarioId, string ueCodigo, string dreCodigo, int bimestre, DateTime dataReferencia)
        {
            var reaberturaPeriodo = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            bimestre,
                                                            dataReferencia,
                                                            tipoCalendarioId,
                                                            dreCodigo,
                                                            ueCodigo);
            return reaberturaPeriodo != null;
        }
    }
}
