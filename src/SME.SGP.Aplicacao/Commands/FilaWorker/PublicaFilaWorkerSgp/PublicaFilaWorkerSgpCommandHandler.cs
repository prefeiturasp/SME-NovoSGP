using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicaFilaWorkerSgpCommandHandler : IRequestHandler<PublicaFilaWorkerSgpCommand, bool>
    {
        private readonly IModel rabbitChannel;

        public PublicaFilaWorkerSgpCommandHandler(IModel rabbitChannel)
        {
            this.rabbitChannel = rabbitChannel ?? throw new ArgumentNullException(nameof(rabbitChannel));
        }

        public Task<bool> Handle(PublicaFilaWorkerSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(request.Filtros,
                                             request.CodigoCorrelacao,
                                             request.UsuarioLogadoNomeCompleto,
                                             request.UsuarioLogadoRF,
                                             request.PerfilUsuario,
                                             request.NotificarErroUsuario);

            var mensagemJson = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);

            rabbitChannel.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, request.NomeFila);
            rabbitChannel.BasicPublish(RotasRabbit.ExchangeSgp, request.NomeFila, null, body);

            return Task.FromResult(true);
        }
    }
}
