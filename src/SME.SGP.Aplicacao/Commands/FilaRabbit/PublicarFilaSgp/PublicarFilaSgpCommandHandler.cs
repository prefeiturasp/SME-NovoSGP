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
        private readonly IServicoMensageria servicoMensageria;

        public PublicarFilaSgpCommandHandler(IMediator mediator, IServicoMensageria servicoMensageria)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var usuario = command.Usuario ?? await ObtenhaUsuario();

            var administrador = await mediator.Send(new ObterAdministradorDoSuporteQuery());

            var mensagem = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             usuario.Nome,
                                             usuario.CodigoRf,
                                             usuario.PerfilAtual,
                                             command.NotificarErroUsuario,
                                             administrador.Login);

            await servicoMensageria.Publicar(mensagem, command.Rota, command.Exchange ?? ExchangeSgpRabbit.Sgp, "PublicarFilaSgp");

            return true;
        }

        private async Task<Usuario> ObtenhaUsuario()
        {
            try
            {
                return await mediator.Send(new ObterUsuarioLogadoQuery());
            } 
            catch
            {
                return new Usuario();
            }
        }

    }
}
