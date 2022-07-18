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
            var request = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             command.Usuario?.Nome,
                                             command.Usuario?.CodigoRf,
                                             command.Usuario?.PerfilAtual,
                                             command.NotificarErroUsuario);

            await servicoMensageria.Publicar(mensagem, command.Rota, command.Exchange ?? ExchangeSgpRabbit.Sgp, "PublicarFilaSgp");

            return true;
        }

        private async Task PublicarMensagem(string rota, byte[] body, string exchange = null)
        {
            var _channel = conexaoRabbit.Get();
            try
            {
                var props = _channel.CreateBasicProperties();
                props.Persistent = true;

                _channel.BasicPublish(exchange ?? ExchangeSgpRabbit.Sgp, rota, props, body);
            }
            finally
            {
                conexaoRabbit.Return(_channel);
            }
        }

    }
}
