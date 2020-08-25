using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaSgpCommandHandler : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IModel model;

        public PublicarFilaSgpCommandHandler(IModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var request = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             command.UsuarioLogadoNomeCompleto,
                                             command.UsuarioLogadoRF,
                                             command.PerfilUsuario,
                                             command.NotificarErroUsuario);

            var mensagem = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(mensagem);

            model.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, command.NomeFila);
            model.BasicPublish(RotasRabbit.ExchangeSgp, command.NomeFila, null, body);
            return Task.FromResult(true);
        }
    }
}
