using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosPAPQueryHandler : IRequestHandler<ObterPeriodosPAPQuery, IEnumerable<PeriodosPAPDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPeriodoRelatorioPAP repositorio;

        public ObterPeriodosPAPQueryHandler(IMediator mediator, IRepositorioPeriodoRelatorioPAP repositorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PeriodosPAPDto>> Handle(ObterPeriodosPAPQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery() { TurmaCodigo = request.CodigoTurma });

            var periodos = await repositorio.ObterPeriodos(turma.AnoLetivo);

            foreach(var periodo in periodos)
            {
                periodo.PeriodoAberto = await mediator.Send(new PeriodoEstaEmAbertoPAPQuery(periodo.PeriodoRelatorioPAPId, turma));
            }

            return periodos;
        }
    }
}
