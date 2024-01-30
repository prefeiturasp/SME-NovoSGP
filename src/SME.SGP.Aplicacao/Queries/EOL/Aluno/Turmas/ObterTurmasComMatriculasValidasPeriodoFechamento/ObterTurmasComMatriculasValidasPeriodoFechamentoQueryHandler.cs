using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasPeriodoFechamentoQueryHandler : IRequestHandler<ObterTurmasComMatriculasValidasPeriodoFechamentoQuery, IEnumerable<string>>
    {
        private readonly IMediator mediator;
        public ObterTurmasComMatriculasValidasPeriodoFechamentoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasComMatriculasValidasPeriodoFechamentoQuery request, CancellationToken cancellationToken)
        {
            var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(request.AlunoCodigo, request.TurmasCodigos, request.PeriodoInicio, request.PeriodoFim));

            var periodoFechamento = await mediator.Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(request.TipoCalendarioId, request.EhTurmaInfantil, request.Bimestre));
            if (periodoFechamento.NaoEhNulo())
            {
                turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(request.AlunoCodigo, request.TurmasCodigos, periodoFechamento.InicioDoFechamento, periodoFechamento.FinalDoFechamento));
            }

            return turmasComMatriculasValidas;
        }
    }
}
