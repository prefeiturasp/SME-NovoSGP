using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Commands
{
    public class PublicarFilaCommandHandler : AsyncRequestHandler<PublicarFilaCommand>
    {
        private readonly IServicoMensageriaSGP servicoMensageria;
        private readonly IServicoMensageriaMetricas servicoMensageriaMetricas;

        public PublicarFilaCommandHandler(IServicoMensageriaSGP servicoMensageria, IServicoMensageriaMetricas servicoMensageriaMetricas)
        {
            this.servicoMensageria = servicoMensageria ?? throw new System.ArgumentNullException(nameof(servicoMensageria));
            this.servicoMensageriaMetricas = servicoMensageriaMetricas ?? throw new System.ArgumentNullException(nameof(servicoMensageriaMetricas));
        }

        protected override async Task Handle(PublicarFilaCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(request.Mensagem,
                                             Guid.NewGuid(),
                                             null,
                                             null,
                                             null,
                                             false,
                                             null);

            await servicoMensageria.Publicar(mensagem, request.Rota, ExchangeSgpRabbit.Sgp, "PublicarFilaMetrica");
            await servicoMensageriaMetricas.Publicado(request.Rota);
        }
    }
}
