using MediatR;
using RabbitMQ.Client;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
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

        public async Task<bool> Handle(PublicarFilaSgpCommand command, CancellationToken cancellationToken)
        {
            var request = new MensagemRabbit(command.Filtros,
                                             command.CodigoCorrelacao,
                                             command.UsuarioLogadoNomeCompleto,
                                             command.UsuarioLogadoRF,
                                             command.PerfilUsuario,
                                             command.NotificarErroUsuario);

            using (MemoryStream body = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(body, request, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });

                model.QueueBind(RotasRabbit.FilaSgp, RotasRabbit.ExchangeSgp, command.NomeFila);
                model.BasicPublish(RotasRabbit.ExchangeSgp, command.NomeFila, null, body.ToArray());
                return true;
            }
        }
    }
}
