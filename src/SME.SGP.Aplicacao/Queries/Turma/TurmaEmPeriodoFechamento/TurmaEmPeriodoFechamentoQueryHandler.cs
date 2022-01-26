using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoFechamentoQueryHandler : IRequestHandler<TurmaEmPeriodoFechamentoQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public TurmaEmPeriodoFechamentoQueryHandler(IMediator mediator,
                                                    IRepositorioEventoFechamento repositorioEventoFechamento,
                                                    IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<bool> Handle(TurmaEmPeriodoFechamentoQuery request, CancellationToken cancellationToken)
        {
            var turma = request.Turma;

            var tipoCalendarioId = request.TipoCalendarioId > 0 ?
                request.TipoCalendarioId :
                await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(turma.ModalidadeTipoCalendario, turma.AnoLetivo, turma.Semestre));

            return await UeEmFechamento(turma, tipoCalendarioId, request.Bimestre, request.DataReferencia)
             || await UeEmReaberturaDeFechamento(tipoCalendarioId, turma.Ue.CodigoUe, turma.Ue.Dre.CodigoDre, request.Bimestre, request.DataReferencia);
        }
        private async Task<bool> UeEmFechamento(Turma turma, long tipoCalendarioId, int bimestre, DateTime dataReferencia)
            => await repositorioEventoFechamento.UeEmFechamento(dataReferencia, tipoCalendarioId, turma.EhTurmaInfantil, bimestre);

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
