using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoMensageriaSGP servicoMensageria;
        private readonly IServicoMensageriaMetricas servicoMensageriaMetricas;

        public PublicarFilaSgpCommandHandler(IMediator mediator, IServicoMensageriaSGP servicoMensageria, IServicoMensageriaMetricas servicoMensageriaMetricas)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.servicoMensageriaMetricas = servicoMensageriaMetricas ?? throw new ArgumentNullException(nameof(servicoMensageriaMetricas));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var usuario = command.Usuario ?? await ObtenhaUsuario();

            var administrador = await mediator.Send(ObterAdministradorDoSuporteQuery.Instance);

            var mensagem = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             usuario?.Nome,
                                             usuario?.CodigoRf,
                                             usuario?.PerfilAtual,
                                             command.NotificarErroUsuario,
                                             administrador.Login);

            await servicoMensageria.Publicar(mensagem, command.Rota, command.Exchange ?? ExchangeSgpRabbit.Sgp, "PublicarFilaSgp");
            await servicoMensageriaMetricas.Publicado(command.Rota);
            return true;
        }

        private async Task<Usuario> ObtenhaUsuario()
        {
            try
            {
                return await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            } 
            catch
            {
                return new Usuario();
            }
        }

    }
}
