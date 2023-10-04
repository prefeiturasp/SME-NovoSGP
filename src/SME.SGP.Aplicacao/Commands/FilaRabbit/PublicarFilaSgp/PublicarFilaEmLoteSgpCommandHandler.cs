using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaEmLoteSgpCommandHandler : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IServicoMensageriaSGP servicoMensageria;
        private readonly IMediator mediator;

        public PublicarFilaEmLoteSgpCommandHandler(IServicoMensageriaSGP servicoMensageria, IMediator mediator)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            foreach (var command in request.Commands)
            {
                string usuarioLogadoNomeCompleto = command.Usuario?.Nome;
                string usuarioLogadoRf = command.Usuario?.CodigoRf;
                Guid? perfilUsuario = command.Usuario?.PerfilAtual;
                var administrador = await mediator.Send(ObterAdministradorDoSuporteQuery.Instance);

                if (command.Usuario.EhNulo())
                {
                    var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

                    usuarioLogadoNomeCompleto = usuario.Nome;
                    usuarioLogadoRf = usuario.CodigoRf;
                    perfilUsuario = usuario.PerfilAtual;
                }

                var mensagem = new MensagemRabbit(command.Filtros,
                                                  command.CodigoCorrelacao,
                                                  usuarioLogadoNomeCompleto,
                                                  usuarioLogadoRf,
                                                  perfilUsuario,
                                                  command.NotificarErroUsuario,
                                                  administrador.Login);

                await servicoMensageria.Publicar(mensagem
                                               , command.Rota
                                               , ExchangeSgpRabbit.Sgp
                                               , "PublicarFilaSgpLote");

            }

            return true;
        }
    }
}
