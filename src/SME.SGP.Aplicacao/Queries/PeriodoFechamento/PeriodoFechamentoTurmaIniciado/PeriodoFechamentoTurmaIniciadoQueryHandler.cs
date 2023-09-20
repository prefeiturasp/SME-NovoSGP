using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PeriodoFechamentoTurmaIniciadoQueryHandler : IRequestHandler<PeriodoFechamentoTurmaIniciadoQuery, bool>
    {
        private readonly IMediator mediator;

        public PeriodoFechamentoTurmaIniciadoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PeriodoFechamentoTurmaIniciadoQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendarioId = request.TipoCalendarioId.HasValue ?
                request.TipoCalendarioId.Value :
                await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(request.Turma));

            int bimestre = request.Bimestre > 0 ? request.Bimestre :
                        request.Turma.ModalidadeTipoCalendario == (ModalidadeTipoCalendario.EJA) ? 2 : 4;

            var periodoFechamento = await mediator.Send(
                new ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery(tipoCalendarioId,
                                                                          bimestre,
                                                                          request.Turma.Ue.DreId,
                                                                          request.Turma.UeId));

            return periodoFechamento.EhNulo() || periodoFechamento.InicioDoFechamento <= request.DataReferencia;
        }
    }
}
