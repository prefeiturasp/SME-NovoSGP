using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaApiEOLCommandHandler : IRequestHandler<PublicarFilaApiEOLCommand, bool>
    {
        private readonly IServicoMensageriaApiEOL servicoMensageria;
        private readonly IServicoMensageriaMetricas servicoMensageriaMetricas;

        public PublicarFilaApiEOLCommandHandler(IServicoMensageriaApiEOL servicoMensageria, IServicoMensageriaMetricas servicoMensageriaMetricas)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.servicoMensageriaMetricas = servicoMensageriaMetricas ?? throw new ArgumentNullException(nameof(servicoMensageriaMetricas));
        }

        public async Task<bool> Handle(PublicarFilaApiEOLCommand command, CancellationToken cancellationToken)
        {
            await servicoMensageria.Publicar(command.Filtros, command.Rota, command.Exchange ?? ExchangeApiEOLRabbit.ApiEOL, "PublicarFilaApiEOL");
            await servicoMensageriaMetricas.Publicado(command.Rota);
            return true;
        }
    }
}
