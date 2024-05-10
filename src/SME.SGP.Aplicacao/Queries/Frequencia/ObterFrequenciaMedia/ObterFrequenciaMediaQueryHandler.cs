using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaMediaQueryHandler : IRequestHandler<ObterFrequenciaMediaQuery, double>
    {
        private readonly IMediator mediator;

        public ObterFrequenciaMediaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<double> Handle(ObterFrequenciaMediaQuery request, CancellationToken cancellationToken)
        {
            if (request.Disciplina.Regencia || !request.Disciplina.LancaNota)
                return double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, request.AnoLetivo)));
          
            return double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.CompensacaoAusenciaPercentualFund2, request.AnoLetivo)));
        }
    }
}
