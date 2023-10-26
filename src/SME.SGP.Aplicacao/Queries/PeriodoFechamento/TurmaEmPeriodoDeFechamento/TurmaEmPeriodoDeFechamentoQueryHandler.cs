using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoDeFechamentoQueryHandler : IRequestHandler<TurmaEmPeriodoDeFechamentoQuery, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEventoFechamentoConsulta repositorioEventoFechamento;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public TurmaEmPeriodoDeFechamentoQueryHandler(
                                        IMediator mediator,
                                        IRepositorioEventoFechamentoConsulta repositorioEventoFechamento,
                                        IRepositorioFechamentoReabertura repositorioFechamentoReabertura)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<bool> Handle(TurmaEmPeriodoDeFechamentoQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(
                                                                                request.Turma.AnoLetivo,
                                                                                request.Turma.ModalidadeTipoCalendario,
                                                                                request.Turma.Semestre));

            return await TurmaEmPeriodoDeFechamento(request, tipoCalendario);
        }

        private async Task<bool> TurmaEmPeriodoDeFechamento(TurmaEmPeriodoDeFechamentoQuery request, TipoCalendario tipoCalendario)
        {
            var ueEmFechamento = await repositorioEventoFechamento.UeEmFechamento(request.Data, tipoCalendario.Id, request.Turma.EhTurmaInfantil, request.Bimestre);

            bool retorno = ueEmFechamento || await UeEmReaberturaDeFechamento(request, tipoCalendario);
            return retorno;
        }

        private async Task<bool> UeEmReaberturaDeFechamento(TurmaEmPeriodoDeFechamentoQuery request, TipoCalendario tipoCalendario)
        {
            var reaberturaPeriodo = await repositorioFechamentoReabertura.ObterReaberturaFechamentoBimestrePorDataReferencia(
                                                            request.Bimestre,
                                                            request.Data,
                                                            tipoCalendario.Id,
                                                            request.Turma.Ue.Dre.CodigoDre,
                                                            request.Turma.Ue.CodigoUe);
            return reaberturaPeriodo.NaoEhNulo();
        }
    }
}
