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
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;

        public PeriodoFechamentoTurmaIniciadoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PeriodoFechamentoTurmaIniciadoQuery request, CancellationToken cancellationToken)
        {
            var tipoCalendarioId = request.TipoCalendarioId.HasValue ?
                request.TipoCalendarioId.Value :
                await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(request.Turma));

            int bimestreFixo = request.Turma.ModalidadeTipoCalendario.EhEjaOuCelp() ? BIMESTRE_2 : BIMESTRE_4;
            int bimestre = request.Bimestre > 0 ? request.Bimestre : bimestreFixo;

            var periodoFechamento = await mediator.Send(
                new ObterPeriodoFechamentoPorCalendarioDreUeBimestreQuery(tipoCalendarioId,
                                                                          bimestre,
                                                                          request.Turma.Ue.DreId,
                                                                          request.Turma.UeId));

            return periodoFechamento.EhNulo() || periodoFechamento.InicioDoFechamento <= request.DataReferencia;
        }
    }
}
