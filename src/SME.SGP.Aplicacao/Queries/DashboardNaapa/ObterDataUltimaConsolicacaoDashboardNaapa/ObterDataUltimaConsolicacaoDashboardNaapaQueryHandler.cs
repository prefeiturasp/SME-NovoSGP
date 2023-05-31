using MediatR;
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
            var tipos = new long[] { (long)request.Tipo };
            var parametroSistema = (await mediator.Send(new ObterParametrosSistemaPorTiposQuery() { Tipos = tipos })).FirstOrDefault();
            if (parametroSistema != null)
            {
                if (!string.IsNullOrEmpty(parametroSistema.Valor))
                    return DateTime.Parse(parametroSistema.Valor);
            }

            return null;
        }
    }
}
