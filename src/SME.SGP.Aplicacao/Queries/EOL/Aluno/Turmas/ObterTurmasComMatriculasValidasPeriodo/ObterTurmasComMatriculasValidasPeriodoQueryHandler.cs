using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasPeriodoQueryHandler : IRequestHandler<ObterTurmasComMatriculasValidasPeriodoQuery, IEnumerable<string>>
    {
        private readonly IMediator mediator;
        public ObterTurmasComMatriculasValidasPeriodoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<string>> Handle(ObterTurmasComMatriculasValidasPeriodoQuery request, CancellationToken cancellationToken)
        {
            var turmasComMatriculasValidas = await mediator
                .Send(new ObterTurmasComMatriculasValidasQuery(request.AlunoCodigo, request.TurmasCodigos, request.PeriodoInicio, request.PeriodoFim), cancellationToken);

            if (!request.ConsideraPeriodoFechamento)
                return turmasComMatriculasValidas.Distinct();

            var periodoFechamento = await mediator
                .Send(new ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(request.TipoCalendarioId, request.EhTurmaInfantil, request.Bimestre), cancellationToken);

            if (periodoFechamento.NaoEhNulo())
            {
                turmasComMatriculasValidas = await mediator
                    .Send(new ObterTurmasComMatriculasValidasQuery(request.AlunoCodigo, request.TurmasCodigos, periodoFechamento.InicioDoFechamento, periodoFechamento.FinalDoFechamento), cancellationToken);
            }

            return turmasComMatriculasValidas.Distinct();
        }
    }
}
