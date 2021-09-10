using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.GoogleClassroom.Infra;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaEmLoteSgpCommandHandler : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IAsyncPolicy policy;

        public PublicarFilaEmLoteSgpCommandHandler(IConfiguration configuration, IReadOnlyPolicyRegistry<string> registry)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagens = new List<(string rota, byte[] body)>();

            foreach (var command in request.Commands)
            {
                var requisicao = new MensagemRabbit(command.Filtros,
                                                    command.CodigoCorrelacao,
                                                    command.UsuarioLogadoNomeCompleto,
                                                    command.UsuarioLogadoRF,
                                                    command.PerfilUsuario,
                                                    command.NotificarErroUsuario);

                var mensagem = JsonConvert.SerializeObject(requisicao, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var body = Encoding.UTF8.GetBytes(mensagem);

                mensagens.Add((command.Rota, body));
            }

            await policy.ExecuteAsync(() => PublicarMensagens(mensagens));

            return true;
        }

        private async Task PublicarMensagens(IEnumerable<(string rota, byte[] body)> mensagens)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("ConfiguracaoRabbit:HostName").Value,
                UserName = configuration.GetSection("ConfiguracaoRabbit:UserName").Value,
                Password = configuration.GetSection("ConfiguracaoRabbit:Password").Value,
                VirtualHost = configuration.GetSection("ConfiguracaoRabbit:Virtualhost").Value
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    var props = _channel.CreateBasicProperties();
                    props.Persistent = true;

                    foreach (var mensagem in mensagens)
                        _channel.BasicPublish(ExchangeRabbit.Sgp, mensagem.rota, props, mensagem.body);                    
                }
            }
        }
    }
}
