using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaEmLoteSgpCommandHandler : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IServicoMensageria servicoMensageria;
        private readonly IMediator mediator;

        public PublicarFilaEmLoteSgpCommandHandler(IServicoMensageria servicoMensageria, IMediator mediator)
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

                if (command.Usuario == null)
                {
                    var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                    usuarioLogadoNomeCompleto = usuario.Nome;
                    usuarioLogadoRf = usuario.CodigoRf;
                    perfilUsuario = usuario.PerfilAtual;
                }

                var mensagem = new MensagemRabbit(command.Filtros,
                                                  command.CodigoCorrelacao,
                                                  usuarioLogadoNomeCompleto,
                                                  usuarioLogadoRf,
                                                  perfilUsuario,
                                                  command.NotificarErroUsuario);

                await servicoMensageria.Publicar(mensagem
                                               , command.Rota
                                               , ExchangeSgpRabbit.Sgp
                                               , "PublicarFilaSgpLote");

            }

            return true;
        }
    }
}
