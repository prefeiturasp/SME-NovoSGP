using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataUltimaConsolicacaoDashboardNaapaQueryHandler : IRequestHandler<ObterDataUltimaConsolicacaoDashboardNaapaQuery, DateTime?>
    {
        private IMediator mediator;

        public ObterDataUltimaConsolicacaoDashboardNaapaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DateTime?> Handle(ObterDataUltimaConsolicacaoDashboardNaapaQuery request, CancellationToken cancellationToken)
        {
            var parametroSistema = (await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(request.Tipo, request.AnoLetivo)))?.FirstOrDefault();
            
            if (parametroSistema.NaoEhNulo() && !string.IsNullOrEmpty(parametroSistema?.Valor))
                return DateTime.Parse(parametroSistema.Valor);
            
            return null;
        }
    }
}
